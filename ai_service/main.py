import os
import re
from fastapi import FastAPI
from pydantic import BaseModel, Field
from dotenv import load_dotenv
from langchain_community.utilities import SQLDatabase
from langchain_openai import ChatOpenAI
from langchain_community.agent_toolkits import create_sql_agent

app = FastAPI(title="Logistics AI Assistant", version="1.0")

load_dotenv()

# Servis yapılandırması koddan değil, çalışma ortamından alınır. Böylece API anahtarı
# kaynak kodda tutulmaz ve Docker/CI ortamlarında güvenli biçimde enjekte edilebilir.
DB_URI = os.getenv(
    "DATABASE_URI",
    "mssql+pyodbc://localhost/LogisticsPlatformDb?driver=ODBC+Driver+17+for+SQL+Server&Trusted_Connection=yes",
)
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY")
OPENAI_MODEL = os.getenv("OPENAI_MODEL", "gpt-3.5-turbo")

agent_executor = None
knowledge_base: list[dict] = []


def get_agent():
    """Bağımlılıklar hazır olduğunda ajanı ilk istekte oluşturur."""
    global agent_executor

    if agent_executor is not None:
        return agent_executor
    if not OPENAI_API_KEY:
        raise RuntimeError("OPENAI_API_KEY ortam değişkeni ayarlanmamış.")

    print("[AI INITIALIZATION] Veritabanına bağlanılıyor...")
    db = SQLDatabase.from_uri(DB_URI)
    print("[AI INITIALIZATION] LLM ve SQL Agent oluşturuluyor...")
    llm = ChatOpenAI(model=OPENAI_MODEL, temperature=0, api_key=OPENAI_API_KEY)
    agent_executor = create_sql_agent(llm, db=db, agent_type="openai-tools", verbose=True)
    print("[AI INITIALIZATION] SQL Agent hazır!")
    return agent_executor

class UserQuery(BaseModel):
    question: str

class KnowledgeDocument(BaseModel):
    id: str
    title: str
    content: str = Field(min_length=1, max_length=20000)

class SearchQuery(BaseModel):
    question: str
    limit: int = Field(default=3, ge=1, le=10)

class ForecastRequest(BaseModel):
    values: list[float] = Field(min_length=2, max_length=365)

@app.get("/")
def read_root():
    return {"status": "AI Service is running", "ai_ready": agent_executor is not None, "configured": bool(OPENAI_API_KEY)}

@app.get("/health")
def health():
    return {"status": "healthy", "configured": bool(OPENAI_API_KEY)}

@app.post("/api/knowledge")
def index_document(document: KnowledgeDocument):
    """Tenant API'nin daha sonra kalıcı depoya taşıyacağı basit RAG indeksleme sözleşmesi."""
    global knowledge_base
    knowledge_base = [item for item in knowledge_base if item["id"] != document.id]
    knowledge_base.append(document.model_dump())
    return {"indexed": True, "id": document.id, "document_count": len(knowledge_base)}

@app.post("/api/knowledge/search")
def search_knowledge(query: SearchQuery):
    terms = set(re.findall(r"[\\wçğıöşü]+", query.question.lower()))
    scored = []
    for document in knowledge_base:
        content_terms = set(re.findall(r"[\\wçğıöşü]+", f"{document['title']} {document['content']}".lower()))
        score = len(terms & content_terms)
        if score:
            scored.append({"id": document["id"], "title": document["title"], "excerpt": document["content"][:500], "score": score})
    return {"results": sorted(scored, key=lambda item: item["score"], reverse=True)[:query.limit]}

@app.post("/api/forecast")
def forecast(request: ForecastRequest):
    """Basit doğrusal trend; üretimde eğitimli model ile değiştirilebilen güvenli başlangıç noktası."""
    count = len(request.values)
    x_mean = (count - 1) / 2
    y_mean = sum(request.values) / count
    denominator = sum((index - x_mean) ** 2 for index in range(count))
    slope = sum((index - x_mean) * (value - y_mean) for index, value in enumerate(request.values)) / denominator
    next_value = y_mean + slope * (count - x_mean)
    return {"forecast": round(max(0, next_value), 2), "trend_per_period": round(slope, 2), "observations": count, "model": "linear-baseline"}

@app.post("/api/ask")
async def ask_assistant(query: UserQuery):
    try:
        response = get_agent().invoke({"input": query.question})
        return {
            "answer": response["output"],
            "source": "LangChain_SQL_Agent"
        }
    except RuntimeError as error:
        return {"answer": str(error), "source": "System"}
    except Exception as error:
        print(f"[AI ERROR] İstek işlenemedi: {error}")
        return {"answer": "AI isteği işlenirken bir hata oluştu.", "source": "Error"}


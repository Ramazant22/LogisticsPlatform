"use client";

import { FormEvent, useState } from "react";
import axios from "axios";
import { Bot, CornerDownLeft, Loader2, Send, Sparkles, UserRound } from "lucide-react";

type Message = {
  id: number;
  role: "assistant" | "user";
  content: string;
};

const suggestedQuestions = [
  "Bugün gecikme riski olan teslimatlar hangileri?",
  "Bakımı yaklaşan araçları listele.",
  "Operasyon risklerini kısa bir özetle.",
  "En verimsiz rotaları göster."
];

export default function AIAssistantPage() {
  const [question, setQuestion] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [messages, setMessages] = useState<Message[]>([
    {
      id: 1,
      role: "assistant",
      content: "Merhaba. Operasyon, teslimat, rota ve bakım verileriniz hakkında soru sorabilirsiniz."
    }
  ]);

  const askQuestion = async (value: string) => {
    const trimmedQuestion = value.trim();
    if (!trimmedQuestion || isLoading) return;

    const userMessage: Message = { id: Date.now(), role: "user", content: trimmedQuestion };
    setMessages((current) => [...current, userMessage]);
    setQuestion("");
    setIsLoading(true);

    try {
      const response = await axios.post("http://localhost:5243/api/AIAssistant/ask", { question: trimmedQuestion });
      const answer = typeof response.data?.answer === "string" ? response.data.answer : "Yanıt alınamadı.";
      setMessages((current) => [...current, { id: Date.now() + 1, role: "assistant", content: answer }]);
    } catch {
      setMessages((current) => [...current, {
        id: Date.now() + 1,
        role: "assistant",
        content: "AI servisine şu anda erişilemiyor. Servisin çalıştığını ve yapılandırmasının tamamlandığını kontrol edin."
      }]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    void askQuestion(question);
  };

  return (
    <div className="mx-auto max-w-5xl space-y-6 p-8">
      <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
        <div>
          <p className="flex items-center gap-2 text-sm font-semibold text-violet-600"><Sparkles size={17} /> AI Operations Assistant</p>
          <h1 className="mt-1 text-3xl font-bold text-slate-900">Operasyon Asistanı</h1>
          <p className="mt-2 text-slate-500">Operasyon verilerinizi doğal dilde sorgulayın ve hızlı aksiyon önerileri alın.</p>
        </div>
        <span className="inline-flex w-fit items-center gap-2 rounded-full border border-emerald-200 bg-emerald-50 px-3 py-1.5 text-sm font-medium text-emerald-700"><span className="h-2 w-2 rounded-full bg-emerald-500" /> AI bağlantısı gerektiğinde doğrulanır</span>
      </div>

      <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
        <div className="flex items-center gap-3 border-b border-slate-100 bg-slate-50 px-6 py-4"><div className="rounded-xl bg-violet-100 p-2 text-violet-700"><Bot size={21} /></div><div><h2 className="font-semibold text-slate-900">Lojistik danışmanı</h2><p className="text-xs text-slate-500">Yanıtlar, bağlı operasyonel veri kaynaklarına dayanır.</p></div></div>
        <div className="min-h-[360px] space-y-5 p-6">
          {messages.map((message) => (
            <div key={message.id} className={`flex gap-3 ${message.role === "user" ? "justify-end" : "justify-start"}`}>
              {message.role === "assistant" && <div className="h-9 w-9 shrink-0 rounded-full bg-violet-100 p-2 text-violet-700"><Bot size={20} /></div>}
              <p className={`max-w-[80%] whitespace-pre-wrap rounded-2xl px-4 py-3 text-sm leading-6 ${message.role === "user" ? "rounded-br-md bg-blue-600 text-white" : "rounded-bl-md bg-slate-100 text-slate-700"}`}>{message.content}</p>
              {message.role === "user" && <div className="h-9 w-9 shrink-0 rounded-full bg-blue-100 p-2 text-blue-700"><UserRound size={20} /></div>}
            </div>
          ))}
          {isLoading && <div className="flex items-center gap-3 text-sm text-slate-500"><div className="h-9 w-9 rounded-full bg-violet-100 p-2 text-violet-700"><Loader2 className="animate-spin" size={20} /></div>Operasyon verileri analiz ediliyor…</div>}
        </div>
        <form onSubmit={handleSubmit} className="border-t border-slate-100 p-4">
          <label className="sr-only" htmlFor="ai-question">Operasyon sorunuz</label>
          <div className="flex gap-3"><textarea id="ai-question" value={question} onChange={(event) => setQuestion(event.target.value)} onKeyDown={(event) => { if (event.key === "Enter" && !event.shiftKey) { event.preventDefault(); void askQuestion(question); } }} rows={2} placeholder="Örn. Bugün gecikme riski yüksek teslimatlar hangileri?" className="min-h-[56px] flex-1 resize-none rounded-xl border border-slate-200 px-4 py-3 text-sm text-slate-900 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100" />
            <button type="submit" disabled={!question.trim() || isLoading} className="inline-flex h-12 items-center gap-2 self-end rounded-xl bg-violet-600 px-4 font-semibold text-white transition hover:bg-violet-700 disabled:cursor-not-allowed disabled:opacity-50"><Send size={17} /> Sor</button>
          </div>
          <p className="mt-2 flex items-center gap-1 text-xs text-slate-400"><CornerDownLeft size={13} /> Göndermek için Enter, yeni satır için Shift + Enter kullanın.</p>
        </form>
      </section>

      <section><h2 className="mb-3 text-sm font-semibold text-slate-700">Önerilen sorular</h2><div className="grid gap-3 sm:grid-cols-2">{suggestedQuestions.map((suggestion) => <button key={suggestion} type="button" onClick={() => void askQuestion(suggestion)} disabled={isLoading} className="rounded-xl border border-slate-200 bg-white p-4 text-left text-sm font-medium text-slate-700 shadow-sm transition hover:border-violet-200 hover:bg-violet-50 disabled:opacity-50">{suggestion}</button>)}</div></section>
    </div>
  );
}

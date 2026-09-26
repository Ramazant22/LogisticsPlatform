# Logistics Platform SaaS

## Proje Özeti
Çok kiracılı (multi-tenant) yapıya sahip, RAG destekli yapay zeka entegrasyonu barındıran kapsamlı bir lojistik ve sevkiyat yönetim platformudur. Sistem, yüksek performans ve ölçeklenebilirlik sağlamak amacıyla Clean Architecture ve Domain-Driven Design (DDD) prensiplerine sadık kalınarak Modular Monolith mimarisinde tasarlanmıştır.

## Teknoloji Yığını

**Backend**
* .NET 10 (ASP.NET Core Web API)
* Clean Architecture & Domain-Driven Design
* CQRS (MediatR) & Repository Pattern
* ASP.NET SignalR

**Frontend**
* Next.js 16 (App Router) & React 19
* Tailwind CSS

**Veri Yönetimi ve Altyapı**
* Microsoft SQL Server 2022 & Entity Framework Core
* Redis (Distributed Caching)
* RabbitMQ & MassTransit (Event-Driven Architecture)
* Hangfire (Background Jobs)
* Docker & Docker Compose

**Yapay Zeka Servisi**
* Python, FastAPI
* LangChain & OpenAI
* Chroma DB (Vector Database)

## Temel Özellikler
* Multi-Tenant Veri İzolasyonu: Kiracı bazlı veri ayrıştırması ve rol tabanlı erişim kontrolü (RBAC).
* Olay Güdümlü Süreçler (Event-Driven): Teslimat sonrası faturalandırma ve bildirim gibi süreçlerin asenkron yönetimi.
* Gerçek Zamanlı Takip: WebSockets kullanılarak araç konumlarının arayüze anlık aktarımı.
* RAG Tabanlı AI Asistanı: Kurumsal belgelere (PDF) ve veritabanı şemasına dayalı doğal dil işleme yeteneği.
* CI/CD Entegrasyonu: GitHub Actions ile otomatik derleme ve xUnit test akışları.

## Kurulum ve Çalıştırma
Proje bağımlılıkları bütünüyle Dockerize edilmiştir. Geliştirme ortamını yapılandırmak için ana dizinde aşağıdaki komutu çalıştırmanız yeterlidir:

```bash
docker-compose up -d

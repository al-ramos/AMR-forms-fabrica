# 🏭 AMR-Fábrica

> Módulo de chão de fábrica do ecossistema AMR SYSTEM — fichas de produção, ordens de serviço, inspeções e rastreabilidade de peças.

[![CI](https://github.com/alexsandro-ramos/AMR.Forms.Fabrica/actions/workflows/ci.yml/badge.svg)](https://github.com/alexsandro-ramos/AMR.Forms.Fabrica/actions/workflows/ci.yml)
[![Deploy AWS](https://github.com/alexsandro-ramos/AMR.Forms.Fabrica/actions/workflows/deploy-aws.yml/badge.svg)](https://github.com/alexsandro-ramos/AMR.Forms.Fabrica/actions/workflows/deploy-aws.yml)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-18-61DAFB?logo=react)

Parte do [AMR SYSTEM](../README.md) — veja a documentação do ecossistema completo.

---

## ✨ Funcionalidades

- **Fichas de Produção** — registro e acompanhamento de ordens por equipamento
- **Inspeções** — formulários de inspeção com checklist e resultado (OK / NOK)
- **Ordens de Reparo** — abertura, acompanhamento e encerramento com histórico
- **Rastreabilidade** — vinculação peça → ficha → inspeção → reparo
- **Integração** — consome AMR-Core (produtos/peças) via HTTP interno

---

## 🛠️ Stack

| Camada | Tecnologia |
|---|---|
| Backend | .NET 8 + Clean Architecture + CQRS (MediatR) |
| ORM | EF Core 8 + SQLite |
| Frontend | React 18 + TypeScript + Vite + Tailwind CSS |
| Mensageria | RabbitMQ + MassTransit (consumer) |
| Testes | xUnit + Coverlet |

---

## 🚀 Rodando localmente

```powershell
# Backend
cd src/AMR.Forms.Fabrica.API
dotnet run
# → http://localhost:5186/swagger

# Frontend
cd amr-forms-fabrica-web
npm install
npm run dev
# → http://localhost:5174
```

Ou suba o ecossistema completo com `.\automation\start-amr-dev.ps1` na raiz do AMR SYSTEM.

---

## 🏗️ Estrutura

```
src/
├── AMR.Forms.Fabrica.Domain/          # Entidades, value objects, regras
├── AMR.Forms.Fabrica.Application/     # CQRS handlers, DTOs, validadores
├── AMR.Forms.Fabrica.Infrastructure/  # EF Core, SQLite, RabbitMQ consumer
└── AMR.Forms.Fabrica.API/             # Controllers, Program.cs, DI
amr-forms-fabrica-web/                  # React + Vite + TypeScript
tests/AMR.Forms.Fabrica.Tests/          # xUnit + Coverlet
```

---

## ☁️ Deploy

Push para `main` dispara `.github/workflows/deploy-aws.yml`:

1. **Build & Push** — imagens API e Web para ECR (`amr-fabrica-api`, `amr-fabrica-web`)
2. **Deploy ECS** — registra nova task definition + force new deployment no cluster `amr-system`
3. **Health check** — aguarda ALB responder na porta 8082

**AWS Secrets necessários:** `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`

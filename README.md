# 🏭 AMR-Fábrica

> Módulo MES de chão de fábrica do ecossistema AMR SYSTEM — fichas de produção, notas fiscais, veículos, pedidos e estrutura organizacional (filiais, departamentos, estações).

[![CI](https://github.com/al-ramos/AMR-forms-fabrica/actions/workflows/ci.yml/badge.svg)](https://github.com/al-ramos/AMR-forms-fabrica/actions/workflows/ci.yml)
[![Deploy AWS](https://github.com/al-ramos/AMR-forms-fabrica/actions/workflows/deploy-aws.yml/badge.svg)](https://github.com/al-ramos/AMR-forms-fabrica/actions/workflows/deploy-aws.yml)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-18-61DAFB?logo=react)

Parte do [AMR SYSTEM](../README.md) — veja a documentação do ecossistema completo.

---

## ✨ Funcionalidades

- **Fichas de Produção** — registro e acompanhamento de ordens por equipamento, incluindo balança e detalhes de carga
- **Notas Fiscais** — emissão, detalhamento e mensagens fiscais por operação
- **Veículos** — cadastro e controle de veículos de expedição/recepção
- **Pedidos de Fábrica** — pedidos com itens, vinculados a tipos de operação configuráveis
- **Estrutura Organizacional** — Filiais, Departamentos, Business Units e Estações de trabalho
- **Tipos de Operação** — configuração de passos e parâmetros por tipo de operação
- **Integração Financeiro** — saída de ficha → ContaPagar; NF transmitida → ContaReceber (HTTP fail-safe)
- **Integração Core** — recebe pedidos via polling (SincronizacaoPedidosService, 5 min)

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

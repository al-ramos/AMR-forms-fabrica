# AMR-Forms-Fábrica — Contexto para Claude Code

Módulo de chão de fábrica do **AMR SYSTEM** — fichas de produção, notas fiscais, rastreabilidade de veículos e sincronização com ERP.

---

## Stack

| Camada      | Tecnologia                                    |
|-------------|-----------------------------------------------|
| Backend     | .NET 8 · Clean Architecture · CQRS (MediatR) |
| ORM         | EF Core 8 · SQLite (`amr_fabrica.db`)         |
| Frontend    | React 18 · TypeScript · Vite                  |
| Mensageria  | RabbitMQ · MassTransit (consumer)             |
| Testes      | xUnit · Coverlet (4 testes unitários)         |
| Deploy      | AWS ECS · ECR · cluster `amr-system`          |

---

## Portas locais

| Serviço       | URL                               |
|---------------|-----------------------------------|
| API (Swagger) | http://localhost:5186/swagger     |
| Frontend      | http://localhost:5174             |
| AMR-Core ERP  | http://localhost:5000             |
| AMR-Financeiro| http://localhost:5015             |

---

## Comandos essenciais

```bash
# Backend
cd src/AMR.Forms.Fabrica.API
dotnet run

# Frontend
cd amr-forms-fabrica-web
npm install
npm run dev

# Testes
cd tests/AMR.Forms.Fabrica.Tests
dotnet test

# Build completo
dotnet build AMR.Forms.Fabrica.sln
```

---

## Estrutura do projeto

```
src/
├── AMR.Forms.Fabrica.Domain/          # Entidades, interfaces de repositório
├── AMR.Forms.Fabrica.Application/     # CQRS handlers, commands, queries
├── AMR.Forms.Fabrica.Infrastructure/  # EF Core, SQLite, HttpClients externos
└── AMR.Forms.Fabrica.API/             # Controllers, Program.cs, BackgroundService
amr-forms-fabrica-web/                 # React + TypeScript + Vite
tests/AMR.Forms.Fabrica.Tests/         # xUnit
```

### Application — Features mapeadas

| Feature     | Commands                                    | Queries                        |
|-------------|---------------------------------------------|--------------------------------|
| Fichas      | AbrirFicha · AvancarPasso · RegistrarSaida  | GetFichas · GetFichaById       |
| Veículos    | CadastrarVeiculo · EditarVeiculo            | GetVeiculos                    |
| Pedidos     | SincronizarPedidos                          | GetPedidos                     |
| Notas Fiscais | RegistrarTransmissaoNf                    | GetNotasFiscais                |
| Filiais     | —                                           | GetFiliais                     |

---

## Endpoints da API

| Método  | Rota                          | Descrição                                   |
|---------|-------------------------------|---------------------------------------------|
| GET     | /api/Ficha?cdFilial=1         | Lista fichas por filial (+ filtro de data)  |
| GET     | /api/Ficha/{id}               | Detalhe de uma ficha                        |
| POST    | /api/Ficha                    | Abre nova ficha (`AbrirFichaCommand`)       |
| PATCH   | /api/Ficha/{id}/passo         | Avança para o próximo passo                 |
| PATCH   | /api/Ficha/{id}/saida         | Registra saída da ficha                     |
| GET     | /api/Veiculo                  | Lista veículos                              |
| GET     | /api/Veiculo/filial/{id}      | Veículos por filial                         |
| POST    | /api/Veiculo                  | Cadastra veículo                            |
| PUT     | /api/Veiculo/{placa}          | Edita veículo                               |
| GET     | /api/NotaFiscal               | Lista notas fiscais                         |
| GET     | /api/Filial                   | Lista filiais                               |
| GET     | /api/Pedido                   | Lista pedidos sincronizados do ERP          |

---

## Banco de dados

- **SQLite** — arquivo `amr_fabrica.db` na pasta da API
- Migrations aplicadas automaticamente no startup (`db.Database.Migrate()`)
- Seed automático: 3 filiais criadas se a tabela estiver vazia
- String de conexão: `appsettings.json` → `ConnectionStrings.AmrFormasFabrica`

---

## Integrações externas

### AMR-Core (ERP)
- `IErpHttpClient` / `ErpHttpClient` — consome `/api/PedidoVenda/aprovados`
- `SincronizacaoPedidosService` (BackgroundService) — sincroniza pedidos aprovados a cada 5 min (configurável via `ErpCore:IntervaloSincronizacaoMinutos`)
- Filiais sincronizadas: array em `ErpCore:Filiais`

### AMR-Financeiro
- `IFinanceiroHttpClient` / `FinanceiroHttpClient` — timeout 15 s
- Integração: saída de ficha gera `ContaPagar`; NF transmitida gera `ContaReceber` (fail-safe)

---

## Deploy AWS

Push para `main` dispara `.github/workflows/deploy-aws.yml`:

1. Build + push das imagens `amr-fabrica-api` e `amr-fabrica-web` no ECR
2. Registra nova task definition + `force-new-deployment` no cluster `amr-system`
3. Health check no ALB (porta 8082)

**Secrets necessários:** `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`

---

## Convenções do código

- Português para nomes de domínio (`Ficha`, `Filial`, `Veiculo`, `Pedido`, `NotaFiscal`)
- Inglês para nomes de infraestrutura (`Handler`, `Repository`, `Controller`, `Query`, `Command`)
- Repositórios registrados como `Scoped` na DI
- Controllers usam primary constructor com `IMediator`
- Frontend: estilos inline com `React.CSSProperties` (sem CSS externo por componente)

---

## Ecossistema AMR

| Sistema         | Repo                    | Porta local |
|-----------------|-------------------------|-------------|
| AMR-Core        | AMR-Core                | 5000 / 5175 |
| AMR-Financeiro  | AMR-Financeiro          | 5015        |
| AMR-Fábrica     | **este repo**           | 5186 / 5174 |

---

## Estado do Projeto — Sprint 6 ativo (02/06–24/06/2026)

- Infra Terraform unificada provisionada na AWS
- CI/CD GitHub Actions funcionando (deploy-aws.yml)
- 4 testes unitários passando (SincronizarPedidosHandler)
- **Sprint 6 entregues no AMR-Fábrica:**
  - AMR-Fábrica re-deploy confirmado em produção ✅ (02/06/2026)
  - CLAUDE.md criado — contexto persistente para Claude Code ✅ (03/06/2026 · `868c3cd`)
  - Vitest + RTL — 10 testes frontend (VeiculosPage, NotaFiscalPage) ✅ (03/06/2026 · `8c2205a`)
  - CI frontend — job `frontend-test` no ci.yml ✅ (03/06/2026 · `9f9a999`)
  - Rate limiting 100 req/min por IP ✅ (03/06/2026 · `a41c9a6`)
  - Security headers middleware (OWASP) ✅ (03/06/2026 · `e55ca68`)
  - Serilog logs estruturados + request logging ✅ (03/06/2026 · `9766cce`)
  - ErrorHandling ProblemDetails RFC 7807 — middleware global + ResultExtensions ✅ (04/06/2026 · `77a5d8c`)
  - Segurança IP — varredura secrets + AWS Account ID → `${{ secrets.AWS_ACCOUNT_ID }}` + .gitignore + LICENSE BSL 1.1 ✅ (04/06/2026 · `fdf3658`)

---

## Protocolo de Encerramento de Card

Ao concluir qualquer card/tarefa, executar nesta ordem:

1. **Git** — commit descritivo + `git push -u origin <branch>`
2. **Notion card** — atualizar `Entrega` para a data real e adicionar referência do commit no conteúdo da página
3. **Kanban** — atualizar a propriedade `Status` do card no Notion para `✅ Concluído` (via `update_properties`)
4. **CLAUDE.md** — atualizar seção `Estado do Projeto` se houve mudança relevante de contexto
5. **Próximo Card** — identificar o próximo card no Backlog, atualizar `Status` para `▶️ Em andamento` no Notion e atualizar a seção `## Próximo Card` abaixo
6. **Merge para main** — fazer merge do branch de feature para `main` e push, garantindo que o CLAUDE.md atualizado esteja disponível para a próxima sessão

---

## Protocolo de Encerramento de Sessão

Disparado quando o usuário disser **"encerrar sessão"** (ou "fechar sessão", "fim do dia", "encerrando").

Executar em ordem:

1. Consolidar todos os cards concluídos na sessão (título, commit, link Notion)
2. **Kanban** — atualizar `Status` de todos os cards trabalhados na sessão (concluídos → `✅ Concluído`, próximo → `▶️ Em andamento`)
3. Confirmar o Próximo Card atualizado no `CLAUDE.md`
4. Criar **1 evento no Google Calendar** com:
   - Título: `AMR-Fábrica ✅ Sessão DD/MM/YYYY`
   - Data/hora: agora (duração 30 min)
   - Descrição: cards entregues + commits + próximo card
   - Reminder: e-mail 0 minutos antes (notificação imediata)

> Apenas 1 chamada de Calendar por sessão — independente de quantos cards foram feitos.

---

## Próximo Card

Sprint 6 do AMR-Fábrica **100% concluído**. Não há próximo card neste repositório para o Sprint 6.
Próximo sprint: **Sprint 7 — AMR CRM** (25/06 — 08/07/2026) em repositório diferente.

---

## Troubleshooting Frequente

| Problema | Solução |
|---|---|
| Porta errada no backend | Verificar `launchSettings.json` → `applicationUrl: http://localhost:5186` |
| CORS bloqueando frontend | `appsettings.json` → `AllowAnyOrigin` já configurado |
| MediatR não resolve handlers | Usar native registration (`.AddMediatR(...)`), remover `MediatR.Extensions` |
| Vite proxy não funciona | Atualizar `vite.config.ts` com a URL correta do backend |
| EF Core PendingModelChangesWarning | Snapshot com tipos SQL Server vs SQLite — suprimido via `SuppressOnlyWarningsThatWouldBeErrors` |

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

# Personal Finance API

API para controle financeiro pessoal com foco em **boas práticas de backend em C#**: Clean Architecture, CQRS, EF Core + SQLite, validações consistentes e fluxo contábil (competência + fechamento mensal).

---

## Stack

- **.NET** (ASP.NET Core)
- **EF Core** + **SQLite**
- **Swagger/OpenAPI** (endpoints agrupados por Commands/Queries)
- **xUnit** (testes)

---

## Arquitetura

Estrutura em camadas (Clean Architecture):

- `personal_finance.Domain`  
  Entidades e regras de negócio (ex.: `Transaction`, `Budget`, `MonthClosing`)
- `personal_finance.Application`  
  Casos de uso (Commands), Queries (CQRS), interfaces (ports), validações e erros
- `personal_finance.Infrastructure`  
  Persistência (EF Core), repositórios, `AppDbContext`, migrations
- `personal_finance.API`  
  Controllers, DI, Swagger, middleware de exceções
- `personal_finance.Tests`  
  Testes unitários (Domain + Application)

---

## Funcionalidades

- **Accounts**: cadastro e consulta de contas
- **Categories**: categorias de receita/despesa
- **Transactions**:
  - criação de lançamentos (crédito/débito)
  - confirmação/cancelamento
  - filtros/paginação/ordenção em consultas
- **Budgets** (orçamento mensal por categoria):
  - *upsert* (cria/atualiza)
  - restrição para categorias de despesa (Expense)
- **Reports**:
  - resumo mensal
  - saldo geral e por conta
  - por categoria
  - **budget vs actual**
- **Recurring**: base para geração mensal de lançamentos recorrentes
- **Month Close**:
  - fechamento mensal com auto-confirmação opcional
  - bloqueio de operações em competência fechada

---

## Executar localmente

### Pré-requisitos
- .NET SDK instalado
- EF Core Tools (se necessário)

```bash
dotnet tool install --global dotnet-ef
# \# Personal Finance API

# 

# Backend de um sistema pessoal de finanças, com foco em aprendizado e boas práticas de backend C#:

# \*\*Clean Architecture\*\*, \*\*CQRS\*\*, \*\*DDD (na medida)\*\*, \*\*SOLID\*\*, \*\*EF Core + SQLite\*\*, Swagger organizado por grupos e regras contábeis básicas (competência, fechamento mensal).

# 

# ---

# 

# \## ✨ Features

# 

# \### Core

# \- \*\*Accounts\*\*: contas (ex: Nubank Corrente, Nubank Investimentos, Caixinhas)

# \- \*\*Categories\*\*: categorias para receitas/despesas

# \- \*\*Transactions\*\*:

# &nbsp; - criação de lançamentos (crédito/débito)

# &nbsp; - confirmação e cancelamento

# &nbsp; - suporte a `AccountId` e `CategoryId` (quando aplicável)

# &nbsp; - filtros, ordenação e paginação nas consultas

# \- \*\*Budgets\*\* (Orçamento por categoria/mês):

# &nbsp; - \*upsert\* (cria se não existir / atualiza se existir)

# &nbsp; - restrição: orçamento só para categorias de despesa (Expense)

# \- \*\*Reports\*\*:

# &nbsp; - resumo mensal (credits/debits)

# &nbsp; - saldo geral

# &nbsp; - saldo por conta

# &nbsp; - resumo por categoria

# &nbsp; - budget vs actual

# \- \*\*Recurring Transactions\*\* (transações recorrentes): base para gerar lançamentos por competência

# \- \*\*Month Close (Fechamento Mensal)\*\*:

# &nbsp; - fecha um mês e (opcionalmente) confirma transações pendentes

# &nbsp; - bloqueia criação/alteração em competência fechada

# 

# ---

# 

# \## 🧱 Arquitetura

# 

# Estrutura em camadas:

# 

# \- `personal\_finance.Domain`

# &nbsp; - entidades e regras de negócio (ex: `Transaction`, `Budget`, `MonthClosing`)

# \- `personal\_finance.Application`

# &nbsp; - casos de uso (Commands/UseCases), Queries (CQRS), interfaces (ports), exceptions e erros

# &nbsp; - `Services/Guards` contém regras reutilizáveis (ex: `MonthCloseGuard`)

# \- `personal\_finance.Infrastructure`

# &nbsp; - EF Core, SQLite, repositórios (implementações), `AppDbContext`, migrations

# \- `personal\_finance.API`

# &nbsp; - controllers, DI, swagger, middleware de tratamento de erros

# \- `personal\_finance.Tests`

# &nbsp; - testes unitários (Domain + Application)

# 

# ---

# 

# \## ✅ Requisitos

# 

# \- .NET SDK (recomendado: versão LTS usada no projeto)

# \- EF Core Tools (`dotnet-ef`) instalado

# 

# Instalar EF Tools (se necessário):

# ```bash

# dotnet tool install --global dotnet-ef




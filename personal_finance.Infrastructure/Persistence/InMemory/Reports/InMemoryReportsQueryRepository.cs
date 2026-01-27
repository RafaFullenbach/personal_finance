using personal_finance.Application.Interfaces.Reports;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.Queries.Reports.Accounts;
using personal_finance.Application.Queries.Reports.Balance;
using personal_finance.Application.Queries.Reports.Budgets;
using personal_finance.Application.Queries.Reports.CategorySummary;
using personal_finance.Application.Queries.Reports.MonthlySummary;
using personal_finance.Domain.Enums;
using personal_finance.Infrastructure.Persistence.InMemory.Transactions;

namespace personal_finance.Infrastructure.Persistence.InMemory.Reports
{
    public class InMemoryReportsQueryRepository : IReportsQueryRepository
    {
        private readonly InMemoryTransactionRepository _writeRepo;

        public InMemoryReportsQueryRepository(InMemoryTransactionRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public Task<MonthlySummaryDto> GetMonthlySummaryAsync(GetMonthlySummaryQuery query)
        {
            var items = _writeRepo.GetAll()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month)
                .ToList();

            var totalCredits = items
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = items
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            var result = new MonthlySummaryDto
            {
                Year = query.Year,
                Month = query.Month,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = items.Count
            };

            return Task.FromResult(result);
        }

        public Task<BalanceDto> GetBalanceAsync(GetBalanceQuery query)
        {
            var items = _writeRepo.GetAll()
                .Where(t => t.TransactionDate.Date <= query.Date.Date)
                .ToList();

            var totalCredits = items
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = items
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            var result = new BalanceDto
            {
                Date = query.Date.Date,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = items.Count
            };

            return Task.FromResult(result);
        }

        public Task<AccountBalanceDto> GetAccountBalanceAsync(GetAccountBalanceQuery query)
        {
            var items = _writeRepo.GetAll()
                .Where(t =>
                    t.AccountId == query.AccountId &&
                    t.TransactionDate.Date <= query.Date.Date)
                .ToList();

            var totalCredits = items
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = items
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            var result = new AccountBalanceDto
            {
                AccountId = query.AccountId,
                Date = query.Date.Date,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = items.Count
            };

            return Task.FromResult(result);
        }

        // ✅ NOVO: Category summary (InMemory)
        // Observação: como este repo não tem acesso às categorias em memória,
        // retornamos CategoryName/CategoryType como "Unknown" por enquanto.
        public Task<IReadOnlyList<CategorySummaryItemDto>> GetCategorySummaryAsync(GetCategorySummaryQuery query)
        {
            var items = _writeRepo.GetAll()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month)
                .Where(t => t.CategoryId.HasValue)
                .ToList();

            var total = items.Sum(t => t.Amount);
            if (total == 0m)
                total = 0m;

            var result = items
                .GroupBy(t => t.CategoryId!.Value)
                .Select(g =>
                {
                    var totalAmount = g.Sum(t => t.Amount);
                    var pct = total == 0m ? 0m : (totalAmount / total) * 100m;

                    return new CategorySummaryItemDto
                    {
                        CategoryId = g.Key,
                        CategoryName = "Unknown",
                        CategoryType = "Unknown",
                        TotalAmount = totalAmount,
                        TransactionsCount = g.Count(),
                        Percentage = Math.Round(pct, 2)
                    };
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<CategorySummaryItemDto>)result);
        }

        public Task<IReadOnlyList<BudgetVsActualItemDto>> GetBudgetVsActualAsync(GetBudgetVsActualQuery query)
        {
            // Base: transações do mês (somente Debits com CategoryId)
            var txs = _writeRepo.GetAll()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month)
                .Where(t => t.Type == TransactionType.Debit)
                .Where(t => t.CategoryId.HasValue)
                .ToList();

            // Actual por categoria
            var actualMap = txs
                .GroupBy(t => t.CategoryId!.Value)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            // Budgets do mês: como InMemoryReportsQueryRepository não tem store de budgets/categorias,
            // vamos retornar tudo como NoBudget (MVP pra compilar).
            // Depois, se você quiser, a gente injeta InMemoryBudgetRepository + InMemoryCategoryRepository
            // pra preencher Budget e CategoryName corretamente.
            var result = actualMap
                .Select(kvp =>
                {
                    var categoryId = kvp.Key;
                    var actual = kvp.Value;

                    return new BudgetVsActualItemDto
                    {
                        CategoryId = categoryId,
                        CategoryName = "Unknown",
                        Budget = null,
                        Actual = actual,
                        Difference = -actual,
                        PercentageUsed = null,
                        Status = "NoBudget"
                    };
                })
                .OrderByDescending(x => x.Actual)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<BudgetVsActualItemDto>)result);
        }
    }
}

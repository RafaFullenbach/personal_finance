using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Reports;
using personal_finance.Domain.Enums;

namespace personal_finance.Infrastructure.Persistence.InMemory
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
    }
}

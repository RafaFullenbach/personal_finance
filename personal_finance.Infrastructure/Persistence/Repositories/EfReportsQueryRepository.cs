using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Reports;
using personal_finance.Domain.Enums;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfReportsQueryRepository : IReportsQueryRepository
    {
        private readonly AppDbContext _db;

        public EfReportsQueryRepository(AppDbContext db) => _db = db;

        public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(GetMonthlySummaryQuery query)
        {
            var q = _db.Transactions.AsNoTracking()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month);

            var totalCredits = await q.Where(t => t.Type == TransactionType.Credit).SumAsync(t => t.Amount);
            var totalDebits = await q.Where(t => t.Type == TransactionType.Debit).SumAsync(t => t.Amount);
            var count = await q.CountAsync();

            return new MonthlySummaryDto
            {
                Year = query.Year,
                Month = query.Month,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = count
            };
        }

        public async Task<BalanceDto> GetBalanceAsync(GetBalanceQuery query)
        {
            var q = _db.Transactions.AsNoTracking()
                .Where(t => t.TransactionDate.Date <= query.Date.Date);

            var totalCredits = await q.Where(t => t.Type == TransactionType.Credit).SumAsync(t => t.Amount);
            var totalDebits = await q.Where(t => t.Type == TransactionType.Debit).SumAsync(t => t.Amount);
            var count = await q.CountAsync();

            return new BalanceDto
            {
                Date = query.Date.Date,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = count
            };
        }

        public async Task<AccountBalanceDto> GetAccountBalanceAsync(GetAccountBalanceQuery query)
        {
            var q = _db.Transactions.AsNoTracking()
                .Where(t => t.AccountId == query.AccountId && t.TransactionDate.Date <= query.Date.Date);

            var totalCredits = await q.Where(t => t.Type == TransactionType.Credit).SumAsync(t => t.Amount);
            var totalDebits = await q.Where(t => t.Type == TransactionType.Debit).SumAsync(t => t.Amount);
            var count = await q.CountAsync();

            return new AccountBalanceDto
            {
                AccountId = query.AccountId,
                Date = query.Date.Date,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = count
            };
        }
        public async Task<IReadOnlyList<CategorySummaryItemDto>> GetCategorySummaryAsync(GetCategorySummaryQuery query)
        {
            var tx = _db.Transactions.AsNoTracking()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month)
                .Where(t => t.CategoryId != null);

            // filtro opcional por tipo (Expense/Income)
            if (!string.IsNullOrWhiteSpace(query.Type) &&
                Enum.TryParse<CategoryType>(query.Type, true, out var catType))
            {
                tx = tx.Join(_db.Categories.AsNoTracking(),
                        t => t.CategoryId!.Value,
                        c => c.Id,
                        (t, c) => new { t, c })
                    .Where(x => x.c.Type == catType)
                    .Select(x => x.t);
            }

            // totals por tipo (pra % correta por Expense vs Income)
            var baseJoin = tx.Join(_db.Categories.AsNoTracking(),
                t => t.CategoryId!.Value,
                c => c.Id,
                (t, c) => new { t, c });

            var totalsByType = await baseJoin
                .GroupBy(x => x.c.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    Total = g.Sum(x => x.t.Amount)
                })
                .ToListAsync();

            var totalMap = totalsByType.ToDictionary(x => x.Type, x => x.Total);

            var grouped = await baseJoin
                .GroupBy(x => new { x.c.Id, x.c.Name, x.c.Type })
                .Select(g => new
                {
                    g.Key.Id,
                    g.Key.Name,
                    g.Key.Type,
                    TotalAmount = g.Sum(x => x.t.Amount),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();

            var result = grouped.Select(x =>
            {
                var totalForType = totalMap.TryGetValue(x.Type, out var t) ? t : 0m;
                var pct = totalForType == 0m ? 0m : (x.TotalAmount / totalForType) * 100m;

                return new CategorySummaryItemDto
                {
                    CategoryId = x.Id,
                    CategoryName = x.Name,
                    CategoryType = x.Type.ToString(),
                    TotalAmount = x.TotalAmount,
                    TransactionsCount = x.Count,
                    Percentage = Math.Round(pct, 2)
                };
            }).ToList();

            return result.AsReadOnly();
        }
    }
}

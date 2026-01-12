using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Budgets;
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

        public async Task<IReadOnlyList<BudgetVsActualItemDto>> GetBudgetVsActualAsync(GetBudgetVsActualQuery query)
        {
            // Categories Expense (base do relatório)
            var expenseCategories = _db.Categories.AsNoTracking()
                .Where(c => c.IsActive && c.Type == CategoryType.Expense);

            // Actual (somente Debit) agrupado por categoria
            var actuals = await _db.Transactions.AsNoTracking()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month)
                .Where(t => t.CategoryId != null)
                .Where(t => t.Type == TransactionType.Debit)
                .GroupBy(t => t.CategoryId!.Value)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    Actual = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            var actualMap = actuals.ToDictionary(x => x.CategoryId, x => x.Actual);

            // Budgets do mês
            var budgets = await _db.Budgets.AsNoTracking()
                .Where(b => b.Year == query.Year && b.Month == query.Month && b.IsActive)
                .Select(b => new { b.CategoryId, b.LimitAmount })
                .ToListAsync();

            var budgetMap = budgets.ToDictionary(x => x.CategoryId, x => x.LimitAmount);

            // Junta tudo pela lista de categorias Expense
            var categories = await expenseCategories
                .Select(c => new { c.Id, c.Name })
                .OrderBy(c => c.Name)
                .ToListAsync();

            static string GetStatus(decimal? budget, decimal actual, decimal? pct)
            {
                if (budget is null) return "NoBudget";
                if (pct is null) return "NoBudget";

                if (pct < 80m) return "Ok";
                if (pct <= 100m) return "Warning";
                return "Exceeded";
            }

            var result = categories
                .Select(c =>
                {
                    var hasActual = actualMap.TryGetValue(c.Id, out var actual);
                    var hasBudget = budgetMap.TryGetValue(c.Id, out var budgetValue);

                    decimal? budget = hasBudget ? budgetValue : (decimal?)null;
                    decimal actualValue = hasActual ? actual : 0m;

                    decimal? pct = null;
                    if (budget.HasValue && budget.Value > 0m)
                    {
                        pct = Math.Round((actualValue / budget.Value) * 100m, 2);
                    }

                    var diff = budget.HasValue ? budget.Value - actualValue : -actualValue;

                    return new BudgetVsActualItemDto
                    {
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        Budget = budget,
                        Actual = actualValue,
                        Difference = diff,
                        PercentageUsed = pct,
                        Status = GetStatus(budget, actualValue, pct)
                    };
                })
                // Mostra primeiro quem está estourando/alerta, depois OK, depois NoBudget
                .OrderByDescending(x => x.Status == "Exceeded")
                .ThenByDescending(x => x.Status == "Warning")
                .ThenByDescending(x => x.Actual)
                .ToList()
                .AsReadOnly();

            return result;
        }
    }
}

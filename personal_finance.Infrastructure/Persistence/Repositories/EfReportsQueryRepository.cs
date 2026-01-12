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
    }
}

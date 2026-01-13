using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Common;
using personal_finance.Application.Queries.Transactions;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfTransactionQueryRepository : ITransactionQueryRepository
    {
        private readonly AppDbContext _db;

        public EfTransactionQueryRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<TransactionListItemDto>> GetAsync(GetTransactionsQuery query)
        {
            var q = _db.Transactions.AsNoTracking().AsQueryable();

            if (query.Year.HasValue)
                q = q.Where(t => t.CompetenceYear == query.Year.Value);

            if (query.Month.HasValue)
                q = q.Where(t => t.CompetenceMonth == query.Month.Value);

            if (!string.IsNullOrWhiteSpace(query.Type))
                q = q.Where(t => t.Type.ToString().Equals(query.Type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(t => t.Status.ToString().Equals(query.Status, StringComparison.OrdinalIgnoreCase));

            var total = await q.CountAsync();

            var desc = string.Equals(query.Order, "desc", StringComparison.OrdinalIgnoreCase);
            var sortBy = (query.SortBy ?? "transactionDate").ToLowerInvariant();

            q = sortBy switch
            {
                "amount" => desc ? q.OrderByDescending(t => t.Amount) : q.OrderBy(t => t.Amount),
                _ => desc ? q.OrderByDescending(t => t.TransactionDate) : q.OrderBy(t => t.TransactionDate),
            };

            var page = query.Page <= 0 ? 1 : query.Page;
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionListItemDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString(),
                    TransactionDate = t.TransactionDate,
                    CompetenceYear = t.CompetenceYear,
                    CompetenceMonth = t.CompetenceMonth,
                    Description = t.Description,
                    AccountId = t.AccountId,
                    RecurringTemplateId = t.RecurringTemplateId,
                })
                .ToListAsync();

            return new PagedResult<TransactionListItemDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        public async Task<TransactionListItemDto?> GetByIdAsync(Guid id)
        {
            return await _db.Transactions.AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TransactionListItemDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString(),
                    TransactionDate = t.TransactionDate,
                    CompetenceYear = t.CompetenceYear,
                    CompetenceMonth = t.CompetenceMonth,
                    Description = t.Description,
                    AccountId = t.AccountId,
                    RecurringTemplateId = t.RecurringTemplateId,
                })
                .FirstOrDefaultAsync();
        }
    }
}

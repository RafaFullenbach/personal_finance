using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.Queries.Common;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Domain.Enums;

namespace personal_finance.Infrastructure.Persistence.Repositories.Transactions
{
    public class EfTransactionQueryRepository : ITransactionQueryRepository
    {
        private readonly AppDbContext _db;

        public EfTransactionQueryRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<TransactionListItemDto>> GetAsync(GetTransactionsQuery query)
        {
            var q = _db.Transactions.AsNoTracking().AsQueryable();

            if (!query.IncludeTransfers)
                q = q.Where(t => t.TransferId == null);

            if (query.Year.HasValue)
                q = q.Where(t => t.CompetenceYear == query.Year.Value);

            if (query.Month.HasValue)
                q = q.Where(t => t.CompetenceMonth == query.Month.Value);

            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                Enum.TryParse<TransactionType>(query.Type, true, out var typeEnum);
                q = q.Where(t => t.Type == typeEnum);
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                Enum.TryParse<TransactionStatus>(query.Status, true, out var statusEnum);
                q = q.Where(t => t.Status == statusEnum);
            }

            var total = await q.CountAsync();

            var desc = string.Equals(query.Order, "desc", StringComparison.OrdinalIgnoreCase);
            var sortBy = (query.SortBy ?? "transactionDate").Trim().ToLowerInvariant();

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
                    AccountName = _db.Accounts
                        .Where(a => a.Id == t.AccountId)
                        .Select(a => a.Name)
                        .FirstOrDefault(),
                    CategoryId = t.CategoryId,
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
                    AccountName = _db.Accounts
                        .Where(a => a.Id == t.AccountId)
                        .Select(a => a.Name)
                        .FirstOrDefault(),
                    CategoryId = t.CategoryId,
                    RecurringTemplateId = t.RecurringTemplateId,
                })
                .FirstOrDefaultAsync();
        }

        public Task<bool> AnyForAccountAsync(Guid accountId)
            => _db.Transactions.AsNoTracking().AnyAsync(t => t.AccountId == accountId);
    }
}

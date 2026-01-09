using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Common;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryTransactionQueryRepository : ITransactionQueryRepository
    {
        private readonly InMemoryTransactionRepository _writeRepo;

        public InMemoryTransactionQueryRepository(InMemoryTransactionRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public Task<PagedResult<TransactionListItemDto>> GetAsync(GetTransactionsQuery query)
        {
            var items = _writeRepo.GetAll().AsQueryable();

            if (query.Year.HasValue)
                items = items.Where(t => t.CompetenceYear == query.Year.Value);

            if (query.Month.HasValue)
                items = items.Where(t => t.CompetenceMonth == query.Month.Value);

            if (!string.IsNullOrWhiteSpace(query.Type))
                items = items.Where(t => t.Type.ToString().Equals(query.Type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Status))
                items = items.Where(t => t.Status.ToString().Equals(query.Status, StringComparison.OrdinalIgnoreCase));

            // total antes da paginação
            var total = items.Count();

            // ordenação
            var desc = string.Equals(query.Order, "desc", StringComparison.OrdinalIgnoreCase);
            var sortBy = (query.SortBy ?? "transactionDate").ToLowerInvariant();

            items = sortBy switch
            {
                "amount" => desc ? items.OrderByDescending(t => t.Amount) : items.OrderBy(t => t.Amount),
                _ => desc ? items.OrderByDescending(t => t.TransactionDate) : items.OrderBy(t => t.TransactionDate),
            };

            // paginação
            var page = query.Page <= 0 ? 1 : query.Page;
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

            var pagedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(Map)
                .ToList()
                .AsReadOnly();

            var result = new PagedResult<TransactionListItemDto>
            {
                Items = pagedItems,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };

            return Task.FromResult(result);
        }

        private static TransactionListItemDto Map(Transaction t) => new()
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type.ToString(),
            Status = t.Status.ToString(),
            TransactionDate = t.TransactionDate,
            CompetenceYear = t.CompetenceYear,
            CompetenceMonth = t.CompetenceMonth,
            Description = t.Description
        };

        public Task<TransactionListItemDto?> GetByIdAsync(Guid id)
        {
            var t = _writeRepo.GetById(id);

            if (t is null)
                return Task.FromResult<TransactionListItemDto?>(null);

            return Task.FromResult<TransactionListItemDto?>(Map(t));
        }
    }
}

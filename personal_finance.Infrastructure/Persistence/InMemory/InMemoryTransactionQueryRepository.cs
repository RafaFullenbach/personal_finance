using personal_finance.Application.Queries.Transactions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryTransactionQueryRepository : ITransactionQueryRepository
    {
        private readonly InMemoryTransactionRepository _writeRepo;

        public InMemoryTransactionQueryRepository(InMemoryTransactionRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public Task<IReadOnlyList<TransactionListItemDto>> GetAsync(GetTransactionsQuery query)
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

            var result = items
                .OrderByDescending(t => t.TransactionDate)
                .Select(Map)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<TransactionListItemDto>)result);
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
    }
}

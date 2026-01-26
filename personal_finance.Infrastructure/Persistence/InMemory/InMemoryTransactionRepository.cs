using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Domain.Entities;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private static readonly ConcurrentDictionary<Guid, Transaction> _store = new();

        public Task AddAsync(Transaction transaction)
        {
            if (!_store.TryAdd(transaction.Id, transaction))
                throw new InvalidOperationException("Transaction with the same id already exists.");

            return Task.CompletedTask;
        }

        public Task<Transaction?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var transaction);
            return Task.FromResult(transaction);
        }

        public Task UpdateAsync(Transaction transaction)
        {
            _store[transaction.Id] = transaction;
            return Task.CompletedTask;
        }

        public IReadOnlyCollection<Transaction> GetAll()
        {
            return _store.Values.ToList().AsReadOnly();
        }

        public Transaction? GetById(Guid id)
        {
            return _store.TryGetValue(id, out var t) ? t : null;
        }

        public Task<bool> ExistsForRecurringAsync(Guid recurringTemplateId, int year, int month)
        {
            var exists = _store.Values.Any(t =>
                t.RecurringTemplateId.HasValue &&
                t.RecurringTemplateId.Value == recurringTemplateId &&
                t.CompetenceYear == year &&
                t.CompetenceMonth == month);

            return Task.FromResult(exists);
        }

        public Task<IReadOnlyList<Transaction>> GetByCompetenceAsync(int year, int month)
        {
            var list = _store.Values
                .Where(t => t.CompetenceYear == year && t.CompetenceMonth == month)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<Transaction>)list);
        }
    }
}

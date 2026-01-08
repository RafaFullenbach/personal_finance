using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using personal_finance.Application.Interfaces;
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
    }
}

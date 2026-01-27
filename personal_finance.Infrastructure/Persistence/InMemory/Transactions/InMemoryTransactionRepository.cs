using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Domain.Entities;

namespace personal_finance.Infrastructure.Persistence.InMemory.Transactions
{
    public sealed class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly ConcurrentDictionary<Guid, Transaction> _store = new();

        public Task AddAsync(Transaction transaction)
        {
            if (transaction is null)
                throw new ArgumentNullException(nameof(transaction));

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
            if (transaction is null)
                throw new ArgumentNullException(nameof(transaction));

            _store[transaction.Id] = transaction;
            return Task.CompletedTask;
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
                .OrderBy(t => t.TransactionDate) // opcional: previsibilidade em testes
                .ToList();

            return Task.FromResult((IReadOnlyList<Transaction>)list.AsReadOnly());
        }

        public IReadOnlyCollection<Transaction> GetAll()
            => _store.Values.ToList().AsReadOnly();

        public Transaction? GetById(Guid id)
            => _store.TryGetValue(id, out var t) ? t : null;
        public void Clear() => _store.Clear();
    }
}

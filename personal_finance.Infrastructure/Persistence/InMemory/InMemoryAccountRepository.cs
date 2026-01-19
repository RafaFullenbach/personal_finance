using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly ConcurrentDictionary<Guid, Account> _store = new();

        public Task AddAsync(Account account)
        {
            _store[account.Id] = account;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Account account)
        {
            _store[account.Id] = account;
            return Task.CompletedTask;
        }

        public Task<Account?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var account);
            return Task.FromResult(account);
        }

        // helper para leitura (usado pelo query repo)
        public IReadOnlyCollection<Account> GetAll()
            => _store.Values.ToList().AsReadOnly();
    }
}

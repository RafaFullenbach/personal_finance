using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryAccountQueryRepository : IAccountQueryRepository
    {
        private readonly InMemoryAccountRepository _writeRepo;

        public InMemoryAccountQueryRepository(InMemoryAccountRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public Task<IReadOnlyList<AccountListItemDto>> GetAllAsync()
        {
            var items = _writeRepo.GetAll()
                .Select(a => new AccountListItemDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type.ToString(),
                    IsActive = a.IsActive
                })
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<AccountListItemDto>)items);
        }
    }
}

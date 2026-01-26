using personal_finance.Application.Queries.Accounts;
using personal_finance.Application.Queries.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.Accounts
{
    public interface IAccountQueryRepository
    {
        Task<IReadOnlyList<AccountListItemDto>> GetAllAsync();
        Task<AccountListItemDto?> GetByIdAsync(Guid id);

    }
}

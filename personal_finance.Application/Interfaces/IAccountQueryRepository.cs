using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Application.Queries.Accounts;

namespace personal_finance.Application.Interfaces
{
    public interface IAccountQueryRepository
    {
        Task<IReadOnlyList<AccountListItemDto>> GetAllAsync();
    }
}

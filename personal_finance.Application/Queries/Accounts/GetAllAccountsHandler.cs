using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Accounts
{
    public class GetAllAccountsHandler
    {
        private readonly IAccountQueryRepository _repo;

        public GetAllAccountsHandler(IAccountQueryRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyList<AccountListItemDto>> HandleAsync()
        {
            return _repo.GetAllAsync();
        }
    }
}

using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Accounts
{
    public class GetAccountByIdHandler
    {
        private readonly IAccountQueryRepository _repo;

        public GetAccountByIdHandler(IAccountQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<AccountListItemDto> HandleAsync(GetAccountByIdQuery query)
        {
            var item = await _repo.GetByIdAsync(query.Id);

            if (item is null)
                throw NotFoundException.For("Account", query.Id);

            return item;
        }

    }
}

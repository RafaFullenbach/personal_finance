using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
{
    public class GetAccountBalanceHandler
    {
        private readonly IReportsQueryRepository _reports;
        private readonly IAccountRepository _accounts;

        public GetAccountBalanceHandler(IReportsQueryRepository reports, IAccountRepository accounts)
        {
            _reports = reports;
            _accounts = accounts;
        }

        public async Task<AccountBalanceDto?> HandleAsync(GetAccountBalanceQuery query)
        {
            var account = await _accounts.GetByIdAsync(query.AccountId);
            if (account is null)
                throw NotFoundException.For("Account", query.AccountId);

            if (!account.IsActive)
            {
                return null;
            }

            return await _reports.GetAccountBalanceAsync(query);
        }
    }
}

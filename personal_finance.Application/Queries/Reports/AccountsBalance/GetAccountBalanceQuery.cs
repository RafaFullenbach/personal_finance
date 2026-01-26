using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.Accounts
{
    public sealed class GetAccountBalanceQuery
    {
        public Guid AccountId { get; init; }
        public DateTime Date { get; init; }
    }
}

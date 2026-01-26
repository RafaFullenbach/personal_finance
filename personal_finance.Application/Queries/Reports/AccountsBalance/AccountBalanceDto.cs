using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.Accounts
{
    public sealed class AccountBalanceDto
    {
        public Guid AccountId { get; init; }
        public DateTime Date { get; init; }

        public decimal TotalCredits { get; init; }
        public decimal TotalDebits { get; init; }
        public decimal Balance => TotalCredits - TotalDebits;

        public int TransactionsCount { get; init; }
    }
}

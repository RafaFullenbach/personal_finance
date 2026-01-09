using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
{
    public sealed class BalanceDto
    {
        public DateTime Date { get; init; }

        public decimal TotalCredits { get; init; }
        public decimal TotalDebits { get; init; }
        public decimal Balance => TotalCredits - TotalDebits;

        public int TransactionsCount { get; init; }
    }
}

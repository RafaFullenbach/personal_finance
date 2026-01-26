using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.MonthlySummary
{
    public sealed class MonthlySummaryDto
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public decimal TotalCredits { get; init; }
        public decimal TotalDebits { get; init; }
        public decimal Net => TotalCredits - TotalDebits;
        public int TransactionsCount { get; init; }
    }
}

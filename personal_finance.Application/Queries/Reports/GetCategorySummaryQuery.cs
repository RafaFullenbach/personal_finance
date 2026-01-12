using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
{
    public sealed class GetCategorySummaryQuery
    {
        public int Year { get; init; }
        public int Month { get; init; }

        // opcional: Expense/Income/null (todos)
        public string? Type { get; init; }
    }
}

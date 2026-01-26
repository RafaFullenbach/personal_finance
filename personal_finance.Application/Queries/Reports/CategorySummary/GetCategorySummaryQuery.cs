using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.CategorySummary
{
    public sealed class GetCategorySummaryQuery
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public string? Type { get; init; }
    }
}

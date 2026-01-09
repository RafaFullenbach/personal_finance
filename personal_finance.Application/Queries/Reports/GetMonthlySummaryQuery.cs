using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
{
    public sealed class GetMonthlySummaryQuery
    {   
        public int Year { get; init; }
        public int Month { get; init; }
    }
}

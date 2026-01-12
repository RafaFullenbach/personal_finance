using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Budgets
{
    public sealed class GetBudgetVsActualQuery
    {
        public int Year { get; init; }
        public int Month { get; init; }
    }
}

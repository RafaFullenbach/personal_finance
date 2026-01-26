using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.Budgets
{
    public sealed class BudgetVsActualItemDto
    {
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = default!;
        public decimal? Budget { get; init; }
        public decimal Actual { get; init; }
        public decimal Difference { get; init; }
        public decimal? PercentageUsed { get; init; } 
        public string Status { get; init; } = default!; 
    }
}

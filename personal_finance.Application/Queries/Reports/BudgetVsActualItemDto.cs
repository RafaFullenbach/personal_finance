using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
{
    public sealed class BudgetVsActualItemDto
    {
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = default!;
        public decimal? Budget { get; init; }
        public decimal Actual { get; init; }
        public decimal Difference { get; init; } // Budget - Actual (se Budget null, pode ser -Actual)
        public decimal? PercentageUsed { get; init; } // null quando não tem budget
        public string Status { get; init; } = default!; // NoBudget | Ok | Warning | Exceeded
    }
}

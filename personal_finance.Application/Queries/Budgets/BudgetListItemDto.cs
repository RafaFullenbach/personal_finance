using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Budgets
{
    public sealed class BudgetListItemDto
    {
        public Guid Id { get; init; }
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = default!;
        public int Year { get; init; }
        public int Month { get; init; }
        public decimal LimitAmount { get; init; }
        public bool IsActive { get; init; }
    }
}

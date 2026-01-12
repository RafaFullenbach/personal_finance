using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.UpsertBudget
{
    public sealed class UpsertBudgetResult
    {
        public Guid Id { get; init; }
        public Guid CategoryId { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public decimal LimitAmount { get; init; }
        public bool IsActive { get; init; }
        public string Action { get; init; } = default!; // "Created" | "Updated"
    }
}

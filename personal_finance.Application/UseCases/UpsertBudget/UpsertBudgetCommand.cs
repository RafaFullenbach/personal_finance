using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.UpsertBudget
{
    public sealed class UpsertBudgetCommand
    {
        public Guid CategoryId { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public decimal LimitAmount { get; init; }
    }
}

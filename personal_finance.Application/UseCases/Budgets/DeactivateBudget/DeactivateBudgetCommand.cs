using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Budgets.DeactivateBudget
{
    public sealed class DeactivateBudgetCommand
    {
        public Guid Id { get; set; }
    }
}

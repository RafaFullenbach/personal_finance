using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.DeactivateBudget
{
    public sealed class DeactivateBudgetCommand
    {
        public Guid Id { get; set; }
    }
}

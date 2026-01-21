using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Budgets
{
    public sealed class GetBudgetByIdQuery
    {
        public Guid Id { get; init; }
    }
}

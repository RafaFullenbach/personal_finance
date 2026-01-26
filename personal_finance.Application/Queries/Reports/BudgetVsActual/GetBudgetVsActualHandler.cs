using personal_finance.Application.Interfaces.Reports;
using personal_finance.Application.Queries.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.Budgets
{
    public sealed class GetBudgetVsActualHandler
    {
        private readonly IReportsQueryRepository _repo;

        public GetBudgetVsActualHandler(IReportsQueryRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyList<BudgetVsActualItemDto>> HandleAsync(GetBudgetVsActualQuery query)
            => _repo.GetBudgetVsActualAsync(query);
    }
}

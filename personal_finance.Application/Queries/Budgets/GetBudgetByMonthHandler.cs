using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Budgets
{
    public sealed class GetBudgetsByMonthHandler
    {
        private readonly IBudgetQueryRepository _repo;
        public GetBudgetsByMonthHandler(IBudgetQueryRepository repo) => _repo = repo;

        public Task<IReadOnlyList<BudgetListItemDto>> HandleAsync(int year, int month)
            => _repo.GetByMonthAsync(year, month);
    }
}

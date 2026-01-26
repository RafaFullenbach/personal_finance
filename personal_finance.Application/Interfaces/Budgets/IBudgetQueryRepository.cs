using personal_finance.Application.Queries.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.Budgets
{
    public interface IBudgetQueryRepository
    {
        Task<IReadOnlyList<BudgetListItemDto>> GetByMonthAsync(int year, int month);

        Task<BudgetDto?> GetByIdAsync(Guid id);
    }
}

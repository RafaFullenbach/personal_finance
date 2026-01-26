using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.Queries.Reports.Accounts;
using personal_finance.Application.Queries.Reports.Balance;
using personal_finance.Application.Queries.Reports.Budgets;
using personal_finance.Application.Queries.Reports.CategorySummary;
using personal_finance.Application.Queries.Reports.MonthlySummary;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.Reports
{
    public interface IReportsQueryRepository
    {
        Task<MonthlySummaryDto> GetMonthlySummaryAsync(GetMonthlySummaryQuery query);
        Task<BalanceDto> GetBalanceAsync(GetBalanceQuery query);
        Task<AccountBalanceDto> GetAccountBalanceAsync(GetAccountBalanceQuery query);
        Task<IReadOnlyList<CategorySummaryItemDto>> GetCategorySummaryAsync(GetCategorySummaryQuery query);
        Task<IReadOnlyList<BudgetVsActualItemDto>> GetBudgetVsActualAsync(GetBudgetVsActualQuery query);
    }
}

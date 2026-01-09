using personal_finance.Application.Queries.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces
{
    public interface IReportsQueryRepository
    {
        Task<MonthlySummaryDto> GetMonthlySummaryAsync(GetMonthlySummaryQuery query);
        Task<BalanceDto> GetBalanceAsync(GetBalanceQuery query);
    }
}

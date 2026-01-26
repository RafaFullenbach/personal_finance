using personal_finance.Application.Interfaces.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.CategorySummary
{
    public sealed class GetCategorySummaryHandler
    {
        private readonly IReportsQueryRepository _reports;
        public GetCategorySummaryHandler(IReportsQueryRepository reports)
        {
            _reports = reports;
        }

        public Task<IReadOnlyList<CategorySummaryItemDto>> HandleAsync(GetCategorySummaryQuery query)
            => _reports.GetCategorySummaryAsync(query);
    }
}

using personal_finance.Application.Errors;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Exceptions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
{
    public class GetMonthlySummaryHandler
    {
        private readonly IReportsQueryRepository _repo;
        public GetMonthlySummaryHandler(IReportsQueryRepository repo)
        {
            _repo = repo;
        }
        public Task<MonthlySummaryDto> HandleAsync(GetMonthlySummaryQuery query)
        {
            if (query.Month < 1 || query.Month > 12)
            {
                throw ValidationException.Invalid(
                    $"Month '{query.Month}' must be between 1 and 12.",
                    ErrorCodes.ReportInvalidPeriod);
            }

            if (query.Year < 2000 || query.Year > 2100)
            {
                throw ValidationException.Invalid(
                    $"Year '{query.Year}' is out of range.",
                    ErrorCodes.ReportInvalidPeriod);
            }

            return _repo.GetMonthlySummaryAsync(query);
        }
    }
}

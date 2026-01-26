using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Application.Interfaces.Reports;

namespace personal_finance.Application.Queries.Reports.MonthlySummary
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
                    $"O mês '{query.Month}' deve estar entre 1 e 12.",
                    ErrorCodes.ReportInvalidPeriod);
            }

            if (query.Year < 2000 || query.Year > 2100)
            {
                throw ValidationException.Invalid(
                    $"Ano '{query.Year}' está fora da faixa.",
                    ErrorCodes.ReportInvalidPeriod);
            }

            return _repo.GetMonthlySummaryAsync(query);
        }
    }
}

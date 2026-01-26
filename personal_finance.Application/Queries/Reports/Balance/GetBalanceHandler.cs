using personal_finance.Application.Interfaces.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.Balance
{
    public class GetBalanceHandler
    {
        private readonly IReportsQueryRepository _repo;
        public GetBalanceHandler(IReportsQueryRepository repo)
        {
            _repo = repo;
        }
        public Task<BalanceDto> HandleAsync(GetBalanceQuery query)
        {
            return _repo.GetBalanceAsync(query);
        }
    }
}

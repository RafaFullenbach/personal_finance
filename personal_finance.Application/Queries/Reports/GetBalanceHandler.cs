using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports
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
            // Aqui por enquanto não precisa validação complexa.
            // Se quiser, dá pra validar datas muito antigas ou futuras.
            return _repo.GetBalanceAsync(query);
        }
    }
}

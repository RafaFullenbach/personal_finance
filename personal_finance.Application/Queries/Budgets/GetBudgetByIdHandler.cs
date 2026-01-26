using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Budgets
{
    public sealed class GetBudgetByIdHandler
    {
        private readonly IBudgetQueryRepository _repo;

        public GetBudgetByIdHandler(IBudgetQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<BudgetDto> HandleAsync(GetBudgetByIdQuery query)
        {
            var item = await _repo.GetByIdAsync(query.Id);
            if (item is null)
                throw NotFoundException.For("Orçamento", query.Id);

            return item;
        }
    }
}

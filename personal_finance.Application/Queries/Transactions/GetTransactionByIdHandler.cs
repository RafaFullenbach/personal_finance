using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Transactions
{
    public class GetTransactionByIdHandler
    {
        private readonly ITransactionQueryRepository _repo;

        public GetTransactionByIdHandler(ITransactionQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<TransactionListItemDto> HandleAsync(GetTransactionByIdQuery query)
        {
            var item = await _repo.GetByIdAsync(query.Id);

            if (item is null)
                throw NotFoundException.For("Lançamento", query.Id);

            return item;
        }
    }
}

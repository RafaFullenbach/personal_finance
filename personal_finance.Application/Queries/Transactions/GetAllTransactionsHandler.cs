using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Transactions
{
    public class GetAllTransactionsHandler
    {
        private readonly ITransactionQueryRepository _queryRepository;

        public GetAllTransactionsHandler(ITransactionQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public Task<PagedResult<TransactionListItemDto>> HandleAsync(GetTransactionsQuery query)
            => _queryRepository.GetAsync(query);
    }
}

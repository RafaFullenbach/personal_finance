using personal_finance.Application.Interfaces;
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

        public Task<IReadOnlyList<TransactionListItemDto>> HandleAsync(GetTransactionsQuery query)
        {
            return _queryRepository.GetAsync(query);
        }
    }
}

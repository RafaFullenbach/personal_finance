using personal_finance.Application.Queries.Common;
using personal_finance.Application.Queries.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces
{
    public interface ITransactionQueryRepository
    {
        Task<PagedResult<TransactionListItemDto>> GetAsync(GetTransactionsQuery query);
        Task<TransactionListItemDto?> GetByIdAsync(Guid id);
    }
}

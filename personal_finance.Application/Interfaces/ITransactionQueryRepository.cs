using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Application.Queries.Transactions;

namespace personal_finance.Application.Interfaces
{
    public interface ITransactionQueryRepository
    {
        Task<IReadOnlyList<TransactionListItemDto>> GetAsync(GetTransactionsQuery query);
        Task<TransactionListItemDto?> GetByIdAsync(Guid id);
    }
}

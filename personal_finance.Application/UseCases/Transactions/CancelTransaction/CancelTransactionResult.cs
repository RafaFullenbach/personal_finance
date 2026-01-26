using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Transactions.CancelTransaction
{
    public class CancelTransactionResult
    {
        public Guid TransactionId { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}

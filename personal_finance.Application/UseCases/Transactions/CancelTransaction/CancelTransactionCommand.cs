using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Transactions.CancelTransaction
{
    public class CancelTransactionCommand
    {
        public Guid TransactionId { get; init; }
    }
}

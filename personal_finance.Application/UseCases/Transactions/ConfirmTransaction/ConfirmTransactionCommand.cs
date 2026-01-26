using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Transactions.ConfirmTransaction
{
    public class ConfirmTransactionCommand
    {
        public Guid TransactionId { get; init; }
    }
}

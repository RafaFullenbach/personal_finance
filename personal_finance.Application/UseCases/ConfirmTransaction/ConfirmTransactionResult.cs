using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.ConfirmTransaction
{
    public class ConfirmTransactionResult
    {
        public Guid TransactionId { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}

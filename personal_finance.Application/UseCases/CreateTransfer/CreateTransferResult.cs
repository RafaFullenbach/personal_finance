using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CreateTransfer
{
    public sealed class CreateTransferResult
    {
        public Guid TransferId { get; init; }
        public Guid DebitTransactionId { get; init; }
        public Guid CreditTransactionId { get; init; }
    }
}

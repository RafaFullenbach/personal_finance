using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Transactions.UpdateTransaction
{
    public sealed class UpdateTransactionResult
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = default!;
    }
}

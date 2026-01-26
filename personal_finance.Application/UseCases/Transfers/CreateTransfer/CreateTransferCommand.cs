using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Transfers.CreateTransfer
{
    public sealed class CreateTransferCommand
    {
        public Guid FromAccountId { get; init; }
        public Guid ToAccountId { get; init; }
        public decimal Amount { get; init; }
        public DateTime TransactionDate { get; init; }
        public int CompetenceYear { get; init; }
        public int CompetenceMonth { get; init; }
        public string Description { get; init; } = default!;
    }
}

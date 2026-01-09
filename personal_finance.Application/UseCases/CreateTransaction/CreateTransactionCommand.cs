using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CreateTransaction
{
    public class CreateTransactionCommand
    {
        public decimal Amount { get; init; }
        public string Type { get; init; } = string.Empty;
        public DateTime TransactionDate { get; init; }
        public int CompetenceYear { get; init; }
        public int CompetenceMonth { get; init; }
        public string Description { get; init; } = string.Empty;
        public Guid AccountId { get; init; }

    }
}

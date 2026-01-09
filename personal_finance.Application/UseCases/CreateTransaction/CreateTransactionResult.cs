using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CreateTransaction
{
    public class CreateTransactionResult
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public string Type { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public int CompetenceYear { get; init; }
        public int CompetenceMonth { get; init; }
        public DateTime TransactionDate { get; init; }
        public string Description { get; init; } = string.Empty;

        public Guid? AccountId { get; init; }
    }
}

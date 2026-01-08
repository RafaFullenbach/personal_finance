using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Transactions
{
    public class TransactionListItemDto
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public string Type { get; init; } = default!;
        public string Status { get; init; } = default!;
        public DateTime TransactionDate { get; init; }
        public int CompetenceYear { get; init; }
        public int CompetenceMonth { get; init; }
        public string Description { get; init; } = default!;
    }
}

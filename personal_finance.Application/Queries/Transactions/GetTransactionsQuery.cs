using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Transactions
{
    public sealed class GetTransactionsQuery
    {
        public int? Year { get; init; }
        public int? Month { get; init; }
        public string? Type { get; init; }   
        public string? Status { get; init; }
    }
}

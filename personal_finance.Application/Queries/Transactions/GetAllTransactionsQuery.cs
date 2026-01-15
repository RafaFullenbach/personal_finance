using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Transactions
{
    public sealed class GetAllTransactionsQuery
    {
        public int? Year { get; init; }
        public int? Month { get; init; }

        // filtros textuais (chegam como string do querystring)
        public string? Type { get; init; }     // "Debit" | "Credit"
        public string? Status { get; init; }   // "Pending" | "Confirmed" | "Cancelled"

        // paginação
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;

        // ordenação
        public string SortBy { get; init; } = "transactionDate"; // "transactionDate" | "amount" | "createdAt"
        public string Order { get; init; } = "desc";             // "asc" | "desc"
    }
}

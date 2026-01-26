using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Transactions
{
    public sealed class GetAllTransactionsQuery
    {
        public int? Year { get; init; }
        public int? Month { get; init; }
        public string? Type { get; init; }     
        public string? Status { get; init; }   
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string SortBy { get; init; } = "transactionDate"; 
        public string Order { get; init; } = "desc";           
    }
}

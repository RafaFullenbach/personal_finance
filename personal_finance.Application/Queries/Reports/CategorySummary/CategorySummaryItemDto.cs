using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Reports.CategorySummary
{
    public sealed class CategorySummaryItemDto
    {
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = default!;
        public string CategoryType { get; init; } = default!;
        public decimal TotalAmount { get; init; }
        public int TransactionsCount { get; init; }
        public decimal Percentage { get; init; }
    }
}

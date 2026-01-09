using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Common
{
    public sealed class PagedResult<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public required int Page { get; init; }
        public required int PageSize { get; init; }
        public required int TotalItems { get; init; }
        public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Categories
{
    public sealed class CategoryListItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Type { get; init; } = default!;
        public bool IsActive { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Categories
{
    public sealed class GetCategoryByIdQuery
    {
        public Guid Id { get; init; }
    }
}

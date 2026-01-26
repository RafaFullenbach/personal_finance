using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Categories.CreateCategory
{
    public sealed class CreateCategoryCommand
    {
        public string Name { get; init; } = default!;
        public string Type { get; init; } = default!; 
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.UpdateCategory
{
    public sealed class UpdateCategoryCommand
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = default!;

        public string Type { get; init; } = default!;
        public bool IsActive { get; init; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Categories.ActivateCategory
{
    public sealed class ActivateCategoryResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}

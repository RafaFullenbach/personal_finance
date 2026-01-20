using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.DeactivateCategory
{
    public sealed class DeactivateCategoryResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}

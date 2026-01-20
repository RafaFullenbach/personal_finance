using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.DeactivateCategory
{
    public sealed class DeactivateCategoryCommand
    {
        public Guid Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CreateRecurringTemplate
{
    public sealed class CreateRecurringTemplateResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}

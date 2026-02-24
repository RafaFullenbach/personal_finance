using System;

namespace personal_finance.Application.UseCases.Recurring.DeactivateRecurringTemplate
{
    public sealed class DeactivateRecurringTemplateResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}


using System;

namespace personal_finance.Application.UseCases.Recurring.ActivateRecurringTemplate
{
    public sealed class ActivateRecurringTemplateResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}


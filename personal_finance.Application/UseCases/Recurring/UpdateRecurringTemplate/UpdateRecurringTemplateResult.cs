using System;

namespace personal_finance.Application.UseCases.Recurring.UpdateRecurringTemplate
{
    public sealed class UpdateRecurringTemplateResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Recurring.CreateRecurringTemplate
{
    public sealed class CreateRecurringTemplateCommand
    {
        public decimal Amount { get; init; }
        public string Type { get; init; } = default!;
        public Guid AccountId { get; init; }
        public Guid? CategoryId { get; init; }
        public string Description { get; init; } = default!;
        public int DayOfMonth { get; init; }
        public int CompetenceOffsetMonths { get; init; } = 0;
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}

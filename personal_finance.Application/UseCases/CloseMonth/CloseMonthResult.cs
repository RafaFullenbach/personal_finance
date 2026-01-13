using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CloseMonth
{
    public sealed class CloseMonthResult
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public DateTime ClosedAt { get; init; }
        public int ConfirmedCount { get; init; }
        public string Action { get; init; } = default!; // "Closed" | "AlreadyClosed"
    }
}

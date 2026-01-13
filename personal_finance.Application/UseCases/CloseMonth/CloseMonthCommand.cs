using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CloseMonth
{
    public sealed class CloseMonthCommand
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public bool AutoConfirmPending { get; init; } = true;
    }
}

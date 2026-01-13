using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.GenerateRecurringTransactions
{
    public sealed class GenerateRecurringTransactionsCommand
    {
        public int Year { get; init; }
        public int Month { get; init; }
    }
}

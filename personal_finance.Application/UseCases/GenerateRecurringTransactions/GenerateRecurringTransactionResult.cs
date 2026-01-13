using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.GenerateRecurringTransactions
{
    public sealed class GenerateRecurringTransactionsResult
    {
        public int CreatedCount { get; init; }
        public int SkippedCount { get; init; }
    }
}

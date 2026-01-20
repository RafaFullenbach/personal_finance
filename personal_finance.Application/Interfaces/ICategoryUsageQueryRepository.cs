using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces
{
    public interface ICategoryUsageQueryRepository
    {
        Task<bool> HasTransactionsAsync(Guid categoryId);
        Task<bool> HasBudgetsAsync(Guid categoryId);
        Task<bool> HasRecurringTemplatesAsync(Guid categoryId);
    }
}

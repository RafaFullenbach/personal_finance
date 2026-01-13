using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces
{
    public interface IRecurringTemplateRepository
    {
        Task AddAsync(RecurringTransactionTemplate template);
        Task<RecurringTransactionTemplate?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<RecurringTransactionTemplate>> GetActiveAsync();
        Task UpdateAsync(RecurringTransactionTemplate template);
    }
}

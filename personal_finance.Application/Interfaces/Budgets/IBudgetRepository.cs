using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.Budgets
{
    public interface IBudgetRepository
    {
        Task AddAsync(Budget budget);
        Task<Budget?> GetByCategoryAndMonthAsync(Guid categoryId, int year, int month);
        Task UpdateAsync(Budget budget);
        Task<Budget?> GetByIdAsync(Guid id);
    }
}

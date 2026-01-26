using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Budgets;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories.Budgets
{
    public class EfBudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _db;
        public EfBudgetRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Budget budget)
        {
            _db.Budgets.Add(budget);
            await _db.SaveChangesAsync();
        }

        public Task<Budget?> GetByCategoryAndMonthAsync(Guid categoryId, int year, int month)
        {
            return _db.Budgets.FirstOrDefaultAsync(b =>
                b.CategoryId == categoryId && b.Year == year && b.Month == month);
        }

        public async Task UpdateAsync(Budget budget)
        {
            _db.Budgets.Update(budget);
            await _db.SaveChangesAsync();
        }

        public async Task<Budget?> GetByIdAsync(Guid id)
        {
            return await _db.Budgets.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}

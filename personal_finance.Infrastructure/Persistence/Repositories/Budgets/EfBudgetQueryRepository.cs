using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Budgets;
using personal_finance.Application.Queries.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories.Budgets
{
    public class EfBudgetQueryRepository : IBudgetQueryRepository
    {
        private readonly AppDbContext _db;
        public EfBudgetQueryRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<BudgetListItemDto>> GetByMonthAsync(int year, int month)
        {
            var items = await _db.Budgets.AsNoTracking()
                .Where(b => b.Year == year && b.Month == month)
                .Join(_db.Categories.AsNoTracking(),
                    b => b.CategoryId,
                    c => c.Id,
                    (b, c) => new BudgetListItemDto
                    {
                        Id = b.Id,
                        CategoryId = b.CategoryId,
                        CategoryName = c.Name,
                        Year = b.Year,
                        Month = b.Month,
                        LimitAmount = b.LimitAmount,
                        IsActive = b.IsActive
                    })
                .OrderBy(x => x.CategoryName)
                .ToListAsync();

            return items.AsReadOnly();
        }

        public async Task<BudgetDto?> GetByIdAsync(Guid id)
        {
            return await _db.Budgets.AsNoTracking()
                .Where(b => b.Id == id)
                .Join(_db.Categories.AsNoTracking(),
                    b => b.CategoryId,
                    c => c.Id,
                    (b, c) => new BudgetDto
                    {
                        Id = b.Id,
                        CategoryId = b.CategoryId,
                        CategoryName = c.Name,
                        Year = b.Year,
                        Month = b.Month,
                        LimitAmount = b.LimitAmount,
                        IsActive = b.IsActive
                    })
                .FirstOrDefaultAsync();
        }
    }
}

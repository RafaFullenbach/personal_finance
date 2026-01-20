using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfCategoryQueryRepository : ICategoryQueryRepository
    {
        private readonly AppDbContext _db;
        public EfCategoryQueryRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<CategoryListItemDto>> GetAllAsync(bool includeInactive = false)
        {
            var q = _db.Categories.AsNoTracking().AsQueryable();

            if (!includeInactive)
                q = q.Where(c => c.IsActive);

            var items = await q
                .OrderBy(c => c.Name)
                .Select(c => new CategoryListItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type.ToString(),
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return items.AsReadOnly();
        }

        public async Task<CategoryListItemDto?> GetByIdAsync(Guid id)
        {
            return await _db.Categories.AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryListItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type.ToString(),
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;
        public EfCategoryRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public Task<Category?> GetByIdAsync(Guid id)
            => _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }
}

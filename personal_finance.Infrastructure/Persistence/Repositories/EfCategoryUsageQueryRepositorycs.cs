using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public sealed class EfCategoryUsageQueryRepository : ICategoryUsageQueryRepository
    {
        private readonly AppDbContext _db;
        public EfCategoryUsageQueryRepository(AppDbContext db) => _db = db;

        public Task<bool> HasTransactionsAsync(Guid categoryId)
            => _db.Transactions.AsNoTracking().AnyAsync(t => t.CategoryId == categoryId);

        public Task<bool> HasBudgetsAsync(Guid categoryId)
            => _db.Budgets.AsNoTracking().AnyAsync(b => b.CategoryId == categoryId);

        public Task<bool> HasRecurringTemplatesAsync(Guid categoryId)
            => _db.RecurringTransactionTemplates.AsNoTracking().AnyAsync(r => r.CategoryId == categoryId);
    }
}

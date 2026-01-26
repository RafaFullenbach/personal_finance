using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Recurring;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories.Recurring
{
    public class EfRecurringTemplateRepository : IRecurringTemplateRepository
    {
        private readonly AppDbContext _db;

        public EfRecurringTemplateRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(RecurringTransactionTemplate template)
        {
            _db.RecurringTransactionTemplates.Add(template);
            await _db.SaveChangesAsync();
        }

        public Task<RecurringTransactionTemplate?> GetByIdAsync(Guid id)
        {
            return _db.RecurringTransactionTemplates.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<RecurringTransactionTemplate>> GetActiveAsync()
        {
            var items = await _db.RecurringTransactionTemplates.AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            return items.AsReadOnly();
        }

        public async Task UpdateAsync(RecurringTransactionTemplate template)
        {
            _db.RecurringTransactionTemplates.Update(template);
            await _db.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Recurring;
using personal_finance.Application.Queries.Recurring;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories.Recurring
{
    public class EfRecurringTemplateQueryRepository : IRecurringTemplateQueryRepository
    {
        private readonly AppDbContext _db;

        public EfRecurringTemplateQueryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<RecurringTemplateListItemDto>> GetAllAsync(bool includeInactive = false)
        {
            var q = _db.RecurringTransactionTemplates.AsNoTracking().AsQueryable();

            if (!includeInactive)
                q = q.Where(x => x.IsActive);

            var items = await q
                .OrderBy(x => x.Description)
                .Select(x => new RecurringTemplateListItemDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Type = x.Type.ToString(),
                    AccountId = x.AccountId,
                    CategoryId = x.CategoryId,
                    Description = x.Description,
                    DayOfMonth = x.DayOfMonth,
                    CompetenceOffsetMonths = x.CompetenceOffsetMonths,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return items.AsReadOnly();
        }
    }
}

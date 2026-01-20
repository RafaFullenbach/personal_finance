using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Accounts;
using personal_finance.Application.Queries.Transactions;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfAccountQueryRepository : IAccountQueryRepository
    {
        private readonly AppDbContext _db;
        public EfAccountQueryRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<AccountListItemDto>> GetAllAsync()
        {
            var items = await _db.Accounts.AsNoTracking()
                .Select(a => new AccountListItemDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type.ToString(),
                    IsActive = a.IsActive
                })
                .ToListAsync();

            return items.AsReadOnly();
        }

        public async Task<AccountListItemDto?> GetByIdAsync(Guid id)
        {
            return await _db.Accounts.AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AccountListItemDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type.ToString(),
                    IsActive = a.IsActive
                })
                .FirstOrDefaultAsync();
        }
    }
}

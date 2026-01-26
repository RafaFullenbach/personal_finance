using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Domain.Entities;

namespace personal_finance.Infrastructure.Persistence.Repositories.Accounts
{
    public class EfAccountRepository : IAccountRepository
    {
        private readonly AppDbContext _db;

        public EfAccountRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Account account)
        {
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();
        }

        public Task<Account?> GetByIdAsync(Guid id)
        {
            return _db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfTransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;

        public EfTransactionRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task<Transaction?> GetByIdAsync(Guid id)
        {
            return await _db.Transactions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
        }
    }
}

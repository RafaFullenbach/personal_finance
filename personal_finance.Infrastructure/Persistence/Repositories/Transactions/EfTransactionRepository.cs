using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Domain.Entities;

namespace personal_finance.Infrastructure.Persistence.Repositories.Transactions
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

        public Task<bool> ExistsForRecurringAsync(Guid recurringTemplateId, int year, int month)
        {
            return _db.Transactions.AsNoTracking().AnyAsync(t =>
                t.RecurringTemplateId == recurringTemplateId &&
                t.CompetenceYear == year &&
                t.CompetenceMonth == month);
        }

        public async Task<IReadOnlyList<Transaction>> GetByCompetenceAsync(int year, int month)
        {
            var list = await _db.Transactions
                .Where(t => t.CompetenceYear == year && t.CompetenceMonth == month)
                .ToListAsync();

            return list.AsReadOnly();
        }
    }
}

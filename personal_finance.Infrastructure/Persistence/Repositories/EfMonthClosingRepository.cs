using Microsoft.EntityFrameworkCore;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.Repositories
{
    public class EfMonthClosingRepository : IMonthClosingRepository
    {
        private readonly AppDbContext _db;
        public EfMonthClosingRepository(AppDbContext db) => _db = db;

        public Task<MonthClosing?> GetByPeriodAsync(int year, int month)
        {
            return _db.MonthClosings.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Year == year && x.Month == month);
        }
        public async Task AddAsync(MonthClosing closing)
        {
            _db.MonthClosings.Add(closing);
            await _db.SaveChangesAsync();
        }
    }
}

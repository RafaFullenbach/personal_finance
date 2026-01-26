using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.CloseMonth
{
    public interface IMonthClosingRepository
    {
        Task<MonthClosing?> GetByPeriodAsync(int year, int month);
        Task AddAsync(MonthClosing closing);
    }
}


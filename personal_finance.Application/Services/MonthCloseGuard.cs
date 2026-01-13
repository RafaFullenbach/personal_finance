using personal_finance.Application.Errors;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Services
{
    public sealed class MonthCloseGuard
    {
        private readonly IMonthClosingRepository _closings;

        public MonthCloseGuard(IMonthClosingRepository closings)
            => _closings = closings;

        public async Task EnsureOpenAsync(int year, int month)
        {
            var closing = await _closings.GetByPeriodAsync(year, month);
            if (closing is not null)
            {
                throw ValidationException.Invalid(
                    $"Month {year:D4}-{month:D2} is closed.",
                    ErrorCodes.MonthClosed);
            }
        }
    }
}

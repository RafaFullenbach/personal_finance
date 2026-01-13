using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;

namespace personal_finance.Application.UseCases.CloseMonth
{
    public sealed class CloseMonthHandler
    {
        private readonly IMonthClosingRepository _closings;
        private readonly ITransactionRepository _transactions;

        public CloseMonthHandler(IMonthClosingRepository closings, ITransactionRepository transactions)
        {
            _closings = closings;
            _transactions = transactions;
        }

        public async Task<CloseMonthResult> HandleAsync(CloseMonthCommand command)
        {
            if (command.Year < 2000 || command.Year > 2100)
                throw ValidationException.Invalid("Invalid year.", ErrorCodes.MonthCloseInvalidPeriod);

            if (command.Month < 1 || command.Month > 12)
                throw ValidationException.Invalid("Invalid month.", ErrorCodes.MonthCloseInvalidPeriod);

            var existing = await _closings.GetByPeriodAsync(command.Year, command.Month);
            if (existing is not null)
            {
                return new CloseMonthResult
                {
                    Year = existing.Year,
                    Month = existing.Month,
                    ClosedAt = existing.ClosedAt,
                    ConfirmedCount = 0,
                    Action = "AlreadyClosed"
                };
            }

            int confirmed = 0;

            if (command.AutoConfirmPending)
            {
                var items = await _transactions.GetByCompetenceAsync(command.Year, command.Month);

                foreach (var t in items)
                {
                    if (t.Status == TransactionStatus.Pending)
                    {
                        t.Confirm();
                        await _transactions.UpdateAsync(t);
                        confirmed++;
                    }
                }
            }

            var closing = new MonthClosing(command.Year, command.Month);
            await _closings.AddAsync(closing);

            return new CloseMonthResult
            {
                Year = closing.Year,
                Month = closing.Month,
                ClosedAt = closing.ClosedAt,
                ConfirmedCount = confirmed,
                Action = "Closed"
            };
        }
    }
}

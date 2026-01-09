using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Reports;
using personal_finance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryReportsQueryRepository : IReportsQueryRepository
    {
        private readonly InMemoryTransactionRepository _writeRepo;

        public InMemoryReportsQueryRepository(InMemoryTransactionRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public Task<MonthlySummaryDto> GetMonthlySummaryAsync(GetMonthlySummaryQuery query)
        {
            var items = _writeRepo.GetAll()
                .Where(t => t.CompetenceYear == query.Year && t.CompetenceMonth == query.Month)
                .ToList();

            var totalCredits = items
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = items
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            var result = new MonthlySummaryDto
            {
                Year = query.Year,
                Month = query.Month,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = items.Count
            };

            return Task.FromResult(result);
        }

        public Task<BalanceDto> GetBalanceAsync(GetBalanceQuery query)
        {
            var items = _writeRepo.GetAll()
                .Where(t => t.TransactionDate.Date <= query.Date.Date)
                .ToList();

            var totalCredits = items
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = items
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            var result = new BalanceDto
            {
                Date = query.Date.Date,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TransactionsCount = items.Count
            };

            return Task.FromResult(result);
        }
    }
}

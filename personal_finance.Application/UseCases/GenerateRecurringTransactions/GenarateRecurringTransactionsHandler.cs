using personal_finance.Application.Errors;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Exceptions;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.GenerateRecurringTransactions
{
    public sealed class GenerateRecurringTransactionsHandler
    {
        private readonly IRecurringTemplateRepository _templates;
        private readonly ITransactionRepository _transactions;

        public GenerateRecurringTransactionsHandler(
            IRecurringTemplateRepository templates,
            ITransactionRepository transactions)
        {
            _templates = templates;
            _transactions = transactions;
        }

        public async Task<GenerateRecurringTransactionsResult> HandleAsync(GenerateRecurringTransactionsCommand command)
        {
            if (command.Month < 1 || command.Month > 12)
                throw ValidationException.Invalid("Invalid month.", ErrorCodes.RecurringInvalidPeriod);

            if (command.Year < 2000 || command.Year > 2100)
                throw ValidationException.Invalid("Invalid year.", ErrorCodes.RecurringInvalidPeriod);

            var active = await _templates.GetActiveAsync();

            // Idempotência: vamos checar no repo se já existe transação daquele template no mês.
            // Você precisa adicionar no ITransactionRepository um método:
            // Task<bool> ExistsForRecurringAsync(Guid templateId, int year, int month);

            int created = 0;
            int skipped = 0;

            foreach (var tpl in active)
            {
                // Verifica se template está válido no período
                var monthStart = new DateTime(command.Year, command.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                if (tpl.StartDate.Date > monthEnd) { skipped++; continue; }
                if (tpl.EndDate.HasValue && tpl.EndDate.Value.Date < monthStart) { skipped++; continue; }

                // Idempotência
                var exists = await _transactions.ExistsForRecurringAsync(tpl.Id, command.Year, command.Month);
                if (exists) { skipped++; continue; }

                var day = tpl.DayOfMonth; // 1..28 garantido
                var txDate = new DateTime(command.Year, command.Month, day);

                // Competência pode ser deslocada
                var competenceDate = new DateTime(command.Year, command.Month, 1).AddMonths(tpl.CompetenceOffsetMonths);

                var tx = new Transaction(
                    amount: tpl.Amount,
                    type: tpl.Type,
                    transactionDate: txDate,
                    competenceYear: competenceDate.Year,
                    competenceMonth: competenceDate.Month,
                    description: tpl.Description,
                    accountId: tpl.AccountId,
                    transferId: null,
                    categoryId: tpl.CategoryId,
                    recurringTemplateId: tpl.Id
                );

                await _transactions.AddAsync(tx);
                created++;
            }

            return new GenerateRecurringTransactionsResult
            {
                CreatedCount = created,
                SkippedCount = skipped
            };
        }
    }
}

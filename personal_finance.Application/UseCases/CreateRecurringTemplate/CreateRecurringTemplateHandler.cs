using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CreateRecurringTemplate
{
    public sealed class CreateRecurringTemplateHandler
    {
        private readonly IRecurringTemplateRepository _repo;
        private readonly IAccountRepository _accounts;
        private readonly ICategoryRepository _categories;

        public CreateRecurringTemplateHandler(
            IRecurringTemplateRepository repo,
            IAccountRepository accounts,
            ICategoryRepository categories)
        {
            _repo = repo;
            _accounts = accounts;
            _categories = categories;
        }

        public async Task<CreateRecurringTemplateResult> HandleAsync(CreateRecurringTemplateCommand command)
        {
            if (command.Amount <= 0)
                throw ValidationException.Invalid("Amount must be greater than zero.", ErrorCodes.RecurringInvalidAmount);

            if (!Enum.TryParse<TransactionType>(command.Type, true, out var type))
                throw ValidationException.Invalid($"Transaction type '{command.Type}' is invalid.", ErrorCodes.RecurringInvalidType);

            var account = await _accounts.GetByIdAsync(command.AccountId);
            if (account is null) throw NotFoundException.For("Account", command.AccountId);

            var category = await _categories.GetByIdAsync(command.CategoryId);
            if (category is null) throw NotFoundException.For("Category", command.CategoryId);

            if (!account.IsActive)
                throw new BusinessRuleException("Account is deactivated. Recurring templates cannot be created.");

            var template = new RecurringTransactionTemplate(
                amount: command.Amount,
                type: type,
                accountId: command.AccountId,
                categoryId: command.CategoryId,
                description: command.Description,
                dayOfMonth: command.DayOfMonth,
                startDate: command.StartDate,
                endDate: command.EndDate,
                competenceOffsetMonths: command.CompetenceOffsetMonths
            );

            await _repo.AddAsync(template);

            return new CreateRecurringTemplateResult
            {
                Id = template.Id,
                IsActive = template.IsActive
            };
        }
    }
}

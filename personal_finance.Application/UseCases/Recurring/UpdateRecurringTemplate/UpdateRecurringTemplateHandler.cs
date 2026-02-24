using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Application.Interfaces.Recurring;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;

namespace personal_finance.Application.UseCases.Recurring.UpdateRecurringTemplate
{
    public sealed class UpdateRecurringTemplateHandler
    {
        private readonly IRecurringTemplateRepository _repo;
        private readonly IAccountRepository _accounts;
        private readonly ICategoryRepository _categories;

        public UpdateRecurringTemplateHandler(
            IRecurringTemplateRepository repo,
            IAccountRepository accounts,
            ICategoryRepository categories)
        {
            _repo = repo;
            _accounts = accounts;
            _categories = categories;
        }

        public async Task<UpdateRecurringTemplateResult> HandleAsync(UpdateRecurringTemplateCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.RecurringInvalidId);

            if (command.Amount <= 0)
                throw ValidationException.Invalid("Valor deve ser maior que zero.", ErrorCodes.RecurringInvalidAmount);

            if (!Enum.TryParse<TransactionType>(command.Type, true, out var type))
                throw ValidationException.Invalid($"Tipo de transação '{command.Type}' é inválido.", ErrorCodes.RecurringInvalidType);

            if (!command.CategoryId.HasValue)
                throw ValidationException.Invalid("Id da categoria é obrigatório.", ErrorCodes.TransactionInvalidCategory);

            var template = await _repo.GetByIdAsync(command.Id);
            if (template is null)
                throw NotFoundException.For("Modelo recorrente", command.Id);

            var account = await _accounts.GetByIdAsync(command.AccountId);
            if (account is null)
                throw NotFoundException.For("Conta", command.AccountId);

            var category = await _categories.GetByIdAsync(command.CategoryId.Value);
            if (category is null)
                throw NotFoundException.For("Categoria", command.CategoryId.Value);

            if (!category.IsActive)
                throw new BusinessRuleException("A categoria está desativada. Não é possível atualizar modelos recorrentes para essa categoria.");

            if (!account.IsActive)
                throw new BusinessRuleException("A conta está desativada. Não é possível atualizar modelos recorrentes para essa conta.");

            try
            {
                template.Update(
                    amount: command.Amount,
                    type: type,
                    accountId: command.AccountId,
                    categoryId: command.CategoryId.Value,
                    description: command.Description,
                    dayOfMonth: command.DayOfMonth,
                    startDate: command.StartDate,
                    endDate: command.EndDate,
                    competenceOffsetMonths: command.CompetenceOffsetMonths);
            }
            catch (ArgumentException ex)
            {
                throw ValidationException.Invalid(ex.Message, ErrorCodes.RecurringInvalidPeriod);
            }

            await _repo.UpdateAsync(template);

            return new UpdateRecurringTemplateResult
            {
                Id = template.Id,
                IsActive = template.IsActive
            };
        }
    }
}


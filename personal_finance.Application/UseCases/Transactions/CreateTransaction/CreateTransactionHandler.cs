using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Application.Interfaces.CloseMonth;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace personal_finance.Application.UseCases.Transactions.CreateTransaction
{
    public class CreateTransactionHandler
    {
        private readonly ITransactionRepository _repository;
        private readonly IAccountRepository _accounts;
        private readonly IMonthClosingRepository _monthClosings;
        private readonly ICategoryRepository _categories;

        public CreateTransactionHandler(
            ITransactionRepository repository,
            IAccountRepository accounts,
            IMonthClosingRepository monthClosings,
            ICategoryRepository categories)

        {
            _repository = repository;
            _accounts = accounts;
            _monthClosings = monthClosings;
            _categories = categories;
            _categories = categories;
        }

        public async Task<CreateTransactionResult> HandleAsync(CreateTransactionCommand command)
        {
            if (command.Amount <= 0)
            {
                throw ValidationException.Invalid(
                    "Valor precisa ser maior que zero.",
                    ErrorCodes.TransactionInvalidAmount);
            }

            if (command.CompetenceMonth < 1 || command.CompetenceMonth > 12)
            {
                throw ValidationException.Invalid(
                    $"Mês de competência '{command.CompetenceMonth}' precisa estar entre 1 e 12.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            if (command.CompetenceYear < 2000 || command.CompetenceYear > 2100)
            {
                throw ValidationException.Invalid(
                    $"Ano de competência '{command.CompetenceYear}' está fora da faixa.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            var closing = await _monthClosings.GetByPeriodAsync(command.CompetenceYear, command.CompetenceMonth);
            if (closing is not null)
            {
                throw ValidationException.Invalid(
                    $"Mês {command.CompetenceYear:D4}-{command.CompetenceMonth:D2} está fechado.",
                    ErrorCodes.MonthClosed);
            }

            if (string.IsNullOrWhiteSpace(command.Description))
            {
                throw ValidationException.Invalid(
                    "Descrição é obrigatória.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            if (command.Description.Length > 200)
            {
                throw ValidationException.Invalid(
                    "Descrição pode ter no máximo 200 caracteres.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            if (!Enum.TryParse<TransactionType>(command.Type, ignoreCase: true, out var type))
            {
                throw ValidationException.Invalid(
                    $"Tipo de lançamento '{command.Type}' é inválido.",
                    ErrorCodes.TransactionInvalidType);
            }

            var account = await _accounts.GetByIdAsync(command.AccountId);
            if (account is null)
                throw NotFoundException.For("Conta", command.AccountId);

            if (!account.IsActive)
                throw new BusinessRuleException("Conta desativada. Não foi possível criar lançamento.");

            if (command.CategoryId.HasValue)
            {
                var category = await _categories.GetByIdAsync(command.CategoryId.Value);
                if (category is null)
                    throw NotFoundException.For("Categoria", command.CategoryId.Value);

                if (!category.IsActive)
                    throw new BusinessRuleException("Categoria desativada. Não foi possível criar transação.");
            }


            var transaction = new Transaction(
                command.Amount,
                type,
                command.TransactionDate,
                command.CompetenceYear,
                command.CompetenceMonth,
                command.Description,
                accountId: command.AccountId,
                transferId: null,
                categoryId: command.CategoryId
            );

            await _repository.AddAsync(transaction);

            return new CreateTransactionResult
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type.ToString(),
                Status = transaction.Status.ToString(),
                CompetenceYear = transaction.CompetenceYear,
                CompetenceMonth = transaction.CompetenceMonth,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description,
                AccountId = transaction.AccountId,
                CategoryId = transaction.CategoryId
            };
        }
    }
}

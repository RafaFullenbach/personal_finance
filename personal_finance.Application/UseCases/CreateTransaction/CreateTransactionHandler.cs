using personal_finance.Application.Errors;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Application.Exceptions;
using System;
using System.Threading.Tasks;

namespace personal_finance.Application.UseCases.CreateTransaction
{
    public class CreateTransactionHandler
    {
        private readonly ITransactionRepository _repository;
        private readonly IAccountRepository _accounts;

        public CreateTransactionHandler(ITransactionRepository repository, IAccountRepository accounts)
        {
            _repository = repository;
            _accounts = accounts;
        }

        public async Task<CreateTransactionResult> HandleAsync(CreateTransactionCommand command)
        {
            // Amount
            if (command.Amount <= 0)
            {
                throw ValidationException.Invalid(
                    "Amount must be greater than zero.",
                    ErrorCodes.TransactionInvalidAmount);
            }

            // Competence month
            if (command.CompetenceMonth < 1 || command.CompetenceMonth > 12)
            {
                throw ValidationException.Invalid(
                    $"CompetenceMonth '{command.CompetenceMonth}' must be between 1 and 12.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            // Competence year
            if (command.CompetenceYear < 2000 || command.CompetenceYear > 2100)
            {
                throw ValidationException.Invalid(
                    $"CompetenceYear '{command.CompetenceYear}' is out of range.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            // Description
            if (string.IsNullOrWhiteSpace(command.Description))
            {
                throw ValidationException.Invalid(
                    "Description is required.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            if (command.Description.Length > 200)
            {
                throw ValidationException.Invalid(
                    "Description must be 200 characters or less.",
                    ErrorCodes.TransactionInvalidCompetence);
            }

            // Type
            if (!Enum.TryParse<TransactionType>(command.Type, ignoreCase: true, out var type))
            {
                throw ValidationException.Invalid(
                    $"Transaction type '{command.Type}' is invalid.",
                    ErrorCodes.TransactionInvalidType);
            }

            // Account exists
            var account = await _accounts.GetByIdAsync(command.AccountId);
            if (account is null)
                throw NotFoundException.For("Account", command.AccountId);

            // ✅ Create transaction including CategoryId (optional)
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

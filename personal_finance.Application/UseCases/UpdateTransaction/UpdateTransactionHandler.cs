using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.UpdateTransaction
{
    public sealed class UpdateTransactionHandler
    {
        private readonly ITransactionRepository _transactions;
        private readonly IAccountRepository _accounts;
        private readonly ICategoryRepository _categories;

        public UpdateTransactionHandler(
            ITransactionRepository transactions,
            IAccountRepository accounts,
            ICategoryRepository categories)
        {
            _transactions = transactions;
            _accounts = accounts;
            _categories = categories;
        }

        public async Task<UpdateTransactionResult> HandleAsync(UpdateTransactionCommand command)
        {
            if (command.Amount <= 0)
                throw ValidationException.Invalid("Amount must be greater than zero.", ErrorCodes.TransactionInvalidAmount);

            if (command.CompetenceMonth < 1 || command.CompetenceMonth > 12)
                throw ValidationException.Invalid("CompetenceMonth must be between 1 and 12.", ErrorCodes.TransactionInvalidCompetence);

            if (command.CompetenceYear < 2000 || command.CompetenceYear > 2100)
                throw ValidationException.Invalid("CompetenceYear is out of range.", ErrorCodes.TransactionInvalidCompetence);

            if (!Enum.TryParse<TransactionType>(command.Type, true, out var type))
                throw ValidationException.Invalid($"Transaction type '{command.Type}' is invalid.", ErrorCodes.TransactionInvalidType);

            var tx = await _transactions.GetByIdAsync(command.Id);
            if (tx is null)
                throw NotFoundException.For("Transaction", command.Id);

            // account exists
            var account = await _accounts.GetByIdAsync(command.AccountId);
            if (account is null)
                throw NotFoundException.For("Account", command.AccountId);

            // category optional
            if (command.CategoryId.HasValue)
            {
                var category = await _categories.GetByIdAsync(command.CategoryId.Value);
                if (category is null)
                    throw NotFoundException.For("Category", command.CategoryId.Value);
            }

            // domain enforcement: only pending
            try
            {
                tx.Update(
                    amount: command.Amount,
                    type: type,
                    transactionDate: command.TransactionDate,
                    competenceYear: command.CompetenceYear,
                    competenceMonth: command.CompetenceMonth,
                    description: command.Description,
                    accountId: command.AccountId,
                    categoryId: command.CategoryId
                );
            }
            catch (InvalidOperationException ex)
            {
                throw new BusinessRuleException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw ValidationException.Invalid(ex.Message, ErrorCodes.TransactionInvalidUpdate);
            }

            await _transactions.UpdateAsync(tx);

            return new UpdateTransactionResult
            {
                Id = tx.Id,
                Status = tx.Status.ToString()
            };
        }
    }
}

using personal_finance.Application.Errors;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.CreateTransaction
{
    public class CreateTransactionHandler
    {
        private readonly ITransactionRepository _repository;

        public CreateTransactionHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateTransactionResult> HandleAsync(CreateTransactionCommand command)
        {
            if (!Enum.TryParse<TransactionType>(command.Type, ignoreCase: true, out var type))
            {
                throw ValidationException.Invalid(
                    $"Transaction type '{command.Type}' is invalid.",
                    ErrorCodes.TransactionInvalidType);
            }

            var transaction = new Transaction(
                command.Amount,
                type,
                command.TransactionDate,
                command.CompetenceYear,
                command.CompetenceMonth,
                command.Description
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
                Description = transaction.Description
            };
        }

    }
}

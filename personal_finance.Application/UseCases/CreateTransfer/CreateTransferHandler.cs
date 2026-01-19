using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace personal_finance.Application.UseCases.CreateTransfer
{
    public class CreateTransferHandler
    {
        private readonly ITransactionRepository _transactions;
        private readonly IAccountRepository _accounts;

        public CreateTransferHandler(ITransactionRepository transactions, IAccountRepository accounts)
        {
            _transactions = transactions;
            _accounts = accounts;
        }

        public async Task<CreateTransferResult> HandleAsync(CreateTransferCommand command)
        {
            if (command.Amount <= 0)
            {
                throw ValidationException.Invalid(
                    "Amount must be greater than zero.",
                    ErrorCodes.TransactionInvalidAmount);
            }

            if (command.FromAccountId == command.ToAccountId)
            {
                throw ValidationException.Invalid(
                    "FromAccountId and ToAccountId must be different.",
                    ErrorCodes.TransferInvalidAccounts);
            }

            var from = await _accounts.GetByIdAsync(command.FromAccountId);
            if (from is null) throw NotFoundException.For("Account", command.FromAccountId);

            var to = await _accounts.GetByIdAsync(command.ToAccountId);
            if (to is null) throw NotFoundException.For("Account", command.ToAccountId);

            if (!from.IsActive)
                throw new BusinessRuleException("Source account is deactivated.");

            if (!to.IsActive)
                throw new BusinessRuleException("Destination account is deactivated.");

            var transferId = Guid.NewGuid();

            var debit = new Transaction(
                command.Amount,
                TransactionType.Debit,
                command.TransactionDate,
                command.CompetenceYear,
                command.CompetenceMonth,
                command.Description,
                accountId: command.FromAccountId,
                transferId: transferId
            );

            var credit = new Transaction(
                command.Amount,
                TransactionType.Credit,
                command.TransactionDate,
                command.CompetenceYear,
                command.CompetenceMonth,
                command.Description,
                accountId: command.ToAccountId,
                transferId: transferId
            );

            await _transactions.AddAsync(debit);
            await _transactions.AddAsync(credit);

            return new CreateTransferResult
            {
                TransferId = transferId,
                DebitTransactionId = debit.Id,
                CreditTransactionId = credit.Id
            };
        }
    }
}

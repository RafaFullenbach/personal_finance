using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace personal_finance.Application.UseCases.Transfers.CreateTransfer
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
                    "Valor deve ser maior que zero.",
                    ErrorCodes.TransactionInvalidAmount);
            }

            if (command.FromAccountId == command.ToAccountId)
            {
                throw ValidationException.Invalid(
                    "As contas devem ser diferentes.",
                    ErrorCodes.TransferInvalidAccounts);
            }

            var from = await _accounts.GetByIdAsync(command.FromAccountId);
            if (from is null) throw NotFoundException.For("Conta", command.FromAccountId);

            var to = await _accounts.GetByIdAsync(command.ToAccountId);
            if (to is null) throw NotFoundException.For("Conta", command.ToAccountId);

            if (!from.IsActive)
                throw new BusinessRuleException("A conta de origem foi desativada");

            if (!to.IsActive)
                throw new BusinessRuleException("A conta de destino foi desativada.");

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

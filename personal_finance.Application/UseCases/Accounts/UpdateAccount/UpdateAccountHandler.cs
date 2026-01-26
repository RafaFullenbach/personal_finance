using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;

namespace personal_finance.Application.UseCases.Accounts.UpdateAccount
{
    public sealed class UpdateAccountHandler
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransactionQueryRepository _transactionQuery;

        public UpdateAccountHandler(IAccountRepository accounts, ITransactionQueryRepository transactionQuery)
        {
            _accounts = accounts;
            _transactionQuery = transactionQuery;
        }

        public async Task<UpdateAccountResult> HandleAsync(UpdateAccountCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.AccountInvalidId);

            if (string.IsNullOrWhiteSpace(command.Name))
                throw ValidationException.Invalid("Nome é obrigatório.", ErrorCodes.AccountInvalidName);

            if (string.IsNullOrWhiteSpace(command.Type))
                throw ValidationException.Invalid("Tipo é obrigatório.", ErrorCodes.AccountInvalidType);

            if (!Enum.TryParse<AccountType>(command.Type, true, out var type))
                throw ValidationException.Invalid($"Conta do tipo '{command.Type}' é inválida.", ErrorCodes.AccountInvalidType);

            var account = await _accounts.GetByIdAsync(command.Id);
            if (account is null)
                throw NotFoundException.For("Conta", command.Id);

            var newType = type;

            if (account.Type != newType)
            {
                var hasTransactions = await _transactionQuery.AnyForAccountAsync(account.Id);
                if (hasTransactions)
                    throw new BusinessRuleException("O Tipo de conta não pode ser alterado porque já existem transações nela.");
            }

            try
            {
                account.Update(command.Name, type);
            }
            catch (ArgumentException ex)
            {
                throw ValidationException.Invalid(ex.Message, ErrorCodes.AccountInvalidName);
            }

            await _accounts.UpdateAsync(account);

            return new UpdateAccountResult
            {
                Id = account.Id,
                Name = account.Name,
                Type = account.Type.ToString(),
                IsActive = account.IsActive
            };
        }
    }
}

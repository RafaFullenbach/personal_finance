using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Accounts.DeactivateAccount
{
    public sealed class DeactivateAccountHandler
    {
        private readonly IAccountRepository _accounts;

        public DeactivateAccountHandler(IAccountRepository accounts)
        {
            _accounts = accounts;
        }

        public async Task<DeactivateAccountResult> HandleAsync(DeactivateAccountCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.AccountInvalidId);

            var account = await _accounts.GetByIdAsync(command.Id);
            if (account is null)
                throw NotFoundException.For("Conta", command.Id);

            account.Deactivate();
            await _accounts.UpdateAsync(account);

            return new DeactivateAccountResult
            {
                Id = account.Id,
                IsActive = account.IsActive
            };
        }
    }
}

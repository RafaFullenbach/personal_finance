using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.DeactivateAccount
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
                throw ValidationException.Invalid("Id is required.", ErrorCodes.AccountInvalidId);

            var account = await _accounts.GetByIdAsync(command.Id);
            if (account is null)
                throw NotFoundException.For("Account", command.Id);

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

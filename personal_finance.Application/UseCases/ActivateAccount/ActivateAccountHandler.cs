using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.ActivateAccount
{
    public sealed class ActivateAccountHandler
    {
        private readonly IAccountRepository _accounts;

        public ActivateAccountHandler(IAccountRepository accounts)
        {
            _accounts = accounts;
        }

        public async Task<ActivateAccountResult> HandleAsync(ActivateAccountCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id is required.", ErrorCodes.AccountInvalidId);

            var account = await _accounts.GetByIdAsync(command.Id);
            if (account is null)
                throw NotFoundException.For("Account", command.Id);

            account.Activate();
            await _accounts.UpdateAsync(account);

            return new ActivateAccountResult
            {
                Id = account.Id,
                IsActive = account.IsActive
            };
        }
    }
}

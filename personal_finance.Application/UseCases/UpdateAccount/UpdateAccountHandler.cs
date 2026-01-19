using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Enums;
using System;

namespace personal_finance.Application.UseCases.UpdateAccount
{
    public sealed class UpdateAccountHandler
    {
        private readonly IAccountRepository _accounts;

        public UpdateAccountHandler(IAccountRepository accounts)
        {
            _accounts = accounts;
        }

        public async Task<UpdateAccountResult> HandleAsync(UpdateAccountCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id is required.", ErrorCodes.AccountInvalidId);

            if (string.IsNullOrWhiteSpace(command.Name))
                throw ValidationException.Invalid("Name is required.", ErrorCodes.AccountInvalidName);

            if (string.IsNullOrWhiteSpace(command.Type))
                throw ValidationException.Invalid("Type is required.", ErrorCodes.AccountInvalidType);

            if (!Enum.TryParse<AccountType>(command.Type, true, out var type))
                throw ValidationException.Invalid($"Account type '{command.Type}' is invalid.", ErrorCodes.AccountInvalidType);

            var account = await _accounts.GetByIdAsync(command.Id);
            if (account is null)
                throw NotFoundException.For("Account", command.Id);

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

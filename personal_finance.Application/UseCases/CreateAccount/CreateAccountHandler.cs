using personal_finance.Application.Errors;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Application.Exceptions;

namespace personal_finance.Application.UseCases.CreateAccount
{
    public class CreateAccountHandler
    {
        private readonly IAccountRepository _repo;

        public CreateAccountHandler(IAccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<CreateAccountResult> HandleAsync(CreateAccountCommand command)
        {
            if (!Enum.TryParse<AccountType>(command.Type, true, out var type))
            {
                throw ValidationException.Invalid(
                    $"Account type '{command.Type}' is invalid.",
                    ErrorCodes.AccountInvalidType);
            }

            var account = new Account(command.Name, type);
            await _repo.AddAsync(account);

            return new CreateAccountResult
            {
                Id = account.Id,
                Name = account.Name,
                Type = account.Type.ToString(),
                IsActive = account.IsActive
            };
        }

    }
}

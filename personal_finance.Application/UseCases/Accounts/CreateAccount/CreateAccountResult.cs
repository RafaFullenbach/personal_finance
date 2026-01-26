using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Accounts.CreateAccount
{
    public sealed class CreateAccountResult
    {   
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Type { get; init; } = default!;
        public bool IsActive { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Accounts.DeactivateAccount
{
    public sealed class DeactivateAccountResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}

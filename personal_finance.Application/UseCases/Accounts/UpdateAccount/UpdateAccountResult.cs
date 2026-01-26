using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Accounts.UpdateAccount
{
    public sealed class UpdateAccountResult
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;

        public string Type { get; init; } = default!;
        public bool IsActive { get; init; }
    }
}

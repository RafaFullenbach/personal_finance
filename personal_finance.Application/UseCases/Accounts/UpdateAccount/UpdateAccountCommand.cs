using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Accounts.UpdateAccount
{
    public sealed class UpdateAccountCommand
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = default!;
        public string? Type { get; init; } = default!;
    }
}

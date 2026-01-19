using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.ActivateAccount
{
    public sealed class ActivateAccountResult
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
    }
}

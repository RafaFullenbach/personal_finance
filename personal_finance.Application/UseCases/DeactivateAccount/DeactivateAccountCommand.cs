using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.DeactivateAccount
{
    public sealed class DeactivateAccountCommand
    {
        public Guid Id { get; set; }
    }
}

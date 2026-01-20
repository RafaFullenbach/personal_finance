using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Accounts
{
    public sealed class GetAccountByIdQuery
    {
        public Guid Id { get; init; }
    }
}

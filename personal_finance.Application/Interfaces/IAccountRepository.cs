using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account);
        Task<Account?> GetByIdAsync(Guid id);
    }
}

using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);

        Task<Transaction?> GetByIdAsync(Guid id);

        Task UpdateAsync(Transaction transaction);
    }
}

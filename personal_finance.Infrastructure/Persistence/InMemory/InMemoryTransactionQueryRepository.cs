using personal_finance.Application.Queries.Transactions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Infrastructure.Persistence.InMemory
{
    public class InMemoryTransactionQueryRepository : ITransactionQueryRepository
    {
        private readonly InMemoryTransactionRepository _writeRepo;

        public InMemoryTransactionQueryRepository(InMemoryTransactionRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public Task<IReadOnlyList<TransactionListItemDto>> GetAllAsync()
        {
            // Precisamos de um jeito de listar as transações guardadas.
            // Então vamos expor um método interno no writeRepo para leitura.
            var items = _writeRepo.GetAll()
                .Select(Map)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<TransactionListItemDto>)items);
        }

        private static TransactionListItemDto Map(Transaction t) => new()
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type.ToString(),
            Status = t.Status.ToString(),
            TransactionDate = t.TransactionDate,
            CompetenceYear = t.CompetenceYear,
            CompetenceMonth = t.CompetenceMonth,
            Description = t.Description
        };
    }
}

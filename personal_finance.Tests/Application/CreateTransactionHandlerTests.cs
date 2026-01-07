using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Tests.Application
{
    public class CreateTransactionHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCreateTransaction_AsPending()
        {
            var repo = new InMemoryTransactionRepository();
            var handler = new CreateTransactionHandler(repo);

            var result = await handler.HandleAsync(new CreateTransactionCommand
            {
                Amount = 150m,
                Type = "Debit",
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Market"
            });

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Pending", result.Status);
            Assert.Equal("Debit", result.Type);
            Assert.Equal(150m, result.Amount);
        }
    }
}

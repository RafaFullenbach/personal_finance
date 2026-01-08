using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Infrastructure.Persistence.InMemory;
using personal_finance.Application.Exceptions;
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

        [Fact]
        public async Task HandleAsync_ShouldThrowValidationException_WhenAmountIsZeroOrNegative()
        {
            var repo = new InMemoryTransactionRepository();
            var handler = new CreateTransactionHandler(repo);

            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await handler.HandleAsync(new CreateTransactionCommand
                {
                    Amount = 0m,
                    Type = "Debit",
                    TransactionDate = DateTime.Today,
                    CompetenceYear = 2026,
                    CompetenceMonth = 1,
                    Description = "Test"
                });
            });

            Assert.Equal("TRANSACTION_INVALID_AMOUNT", ex.Code);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrowValidationException_WhenCompetenceMonthIsOutOfRange()
        {
            var repo = new InMemoryTransactionRepository();
            var handler = new CreateTransactionHandler(repo);

            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await handler.HandleAsync(new CreateTransactionCommand
                {
                    Amount = 10m,
                    Type = "Debit",
                    TransactionDate = DateTime.Today,
                    CompetenceYear = 2026,
                    CompetenceMonth = 13,
                    Description = "Test"
                });
            });

            // Se você usou um code específico, ajuste aqui.
            Assert.Equal("TRANSACTION_INVALID_COMPETENCE", ex.Code);
        }
    }
}

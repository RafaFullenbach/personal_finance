using personal_finance.Application.UseCases.CancelTransaction;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Tests.Application
{
    public class CancelTransactionHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCancelTransaction_WhenPending()
        {
            var repository = new InMemoryTransactionRepository();

            // Arrange - cria transação (Pending)
            var createHandler = new CreateTransactionHandler(repository);
            var created = await createHandler.HandleAsync(new CreateTransactionCommand
            {
                Amount = 100m,
                Type = "Debit",
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Test cancel"
            });

            var cancelHandler = new CancelTransactionHandler(repository);

            // Act - cancela
            var result = await cancelHandler.HandleAsync(new CancelTransactionCommand
            {
                TransactionId = created.Id
            });

            // Assert
            Assert.Equal(created.Id, result.TransactionId);
            Assert.Equal("Cancelled", result.Status);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenTransactionNotFound()
        {
            var repository = new InMemoryTransactionRepository();
            var cancelHandler = new CancelTransactionHandler(repository);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await cancelHandler.HandleAsync(new CancelTransactionCommand
                {
                    TransactionId = Guid.NewGuid()
                });
            });
        }

        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenTransactionAlreadyConfirmed()
        {
            var repository = new InMemoryTransactionRepository();

            // Arrange - cria
            var createHandler = new CreateTransactionHandler(repository);
            var created = await createHandler.HandleAsync(new CreateTransactionCommand
            {
                Amount = 200m,
                Type = "Credit",
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Salary"
            });

            // Confirma
            var confirmHandler = new ConfirmTransactionHandler(repository);
            await confirmHandler.HandleAsync(new ConfirmTransactionCommand
            {
                TransactionId = created.Id
            });

            // Tenta cancelar depois de confirmado
            var cancelHandler = new CancelTransactionHandler(repository);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await cancelHandler.HandleAsync(new CancelTransactionCommand
                {
                    TransactionId = created.Id
                });
            });
        }
    }
}

using personal_finance.Application.Exceptions;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Tests.Application
{
    public class GetTransactionByIdHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnTransaction_WhenExists()
        {
            // Arrange: cria o store em memória compartilhado
            var writeRepo = new InMemoryTransactionRepository();
            var queryRepo = new InMemoryTransactionQueryRepository(writeRepo);

            // Cria uma transação via write repo (método AddAsync já existe)
            var handlerCreate = new personal_finance.Application.UseCases.CreateTransaction.CreateTransactionHandler(writeRepo);
            var created = await handlerCreate.HandleAsync(new personal_finance.Application.UseCases.CreateTransaction.CreateTransactionCommand
            {
                Amount = 100m,
                Type = "Debit",
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Test"
            });

            var handler = new GetTransactionByIdHandler(queryRepo);

            // Act
            var result = await handler.HandleAsync(new GetTransactionByIdQuery { Id = created.Id });

            // Assert
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("Debit", result.Type);
            Assert.Equal("Pending", result.Status);
            Assert.Equal(100m, result.Amount);
            Assert.Equal(2026, result.CompetenceYear);
            Assert.Equal(1, result.CompetenceMonth);
            Assert.Equal("Test", result.Description);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrowNotFoundException_WhenTransactionDoesNotExist()
        {
            // Arrange
            var writeRepo = new InMemoryTransactionRepository();
            var queryRepo = new InMemoryTransactionQueryRepository(writeRepo);
            var handler = new GetTransactionByIdHandler(queryRepo);

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.HandleAsync(new GetTransactionByIdQuery { Id = Guid.NewGuid() });
            });
        }
    }
}

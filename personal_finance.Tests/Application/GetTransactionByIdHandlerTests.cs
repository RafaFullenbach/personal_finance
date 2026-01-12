using System;
using System.Threading.Tasks;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Infrastructure.Persistence.InMemory;
using Xunit;

namespace personal_finance.Tests.Application
{
    public class GetTransactionByIdHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnTransaction_WhenExists()
        {
            // Arrange: cria stores em memória compartilhados
            var writeRepo = new InMemoryTransactionRepository();
            var accountsRepo = new InMemoryAccountRepository();
            var queryRepo = new InMemoryTransactionQueryRepository(writeRepo);

            // cria uma conta válida (AccountId obrigatório)
            var account = new Account("Wallet", AccountType.Cash);
            await accountsRepo.AddAsync(account);

            // Cria uma transação via handler de criação
            var handlerCreate = new personal_finance.Application.UseCases.CreateTransaction.CreateTransactionHandler(writeRepo, accountsRepo);

            var created = await handlerCreate.HandleAsync(new personal_finance.Application.UseCases.CreateTransaction.CreateTransactionCommand
            {
                AccountId = account.Id,
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
            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.HandleAsync(new GetTransactionByIdQuery { Id = Guid.NewGuid() }));
        }
    }
}

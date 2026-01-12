using personal_finance.Application.Exceptions;
using personal_finance.Application.UseCases.CreateTransfer;
using personal_finance.Domain.Enums;
using personal_finance.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Tests.Application
{
    public class CreateTransferHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCreateTwoTransactions_WithSameTransferId()
        {
            // Arrange
            var accountsRepo = new InMemoryAccountRepository();
            var transactionsRepo = new InMemoryTransactionRepository();

            // cria duas contas
            var from = new personal_finance.Domain.Entities.Account("Wallet", AccountType.Cash);
            var to = new personal_finance.Domain.Entities.Account("Nubank", AccountType.Bank);

            await accountsRepo.AddAsync(from);
            await accountsRepo.AddAsync(to);

            var handler = new CreateTransferHandler(transactionsRepo, accountsRepo);

            var cmd = new CreateTransferCommand
            {
                FromAccountId = from.Id,
                ToAccountId = to.Id,
                Amount = 500m,
                TransactionDate = new DateTime(2026, 1, 10),
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Transfer Wallet -> Nubank"
            };

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.NotEqual(Guid.Empty, result.TransferId);
            Assert.NotEqual(Guid.Empty, result.DebitTransactionId);
            Assert.NotEqual(Guid.Empty, result.CreditTransactionId);

            var debit = await transactionsRepo.GetByIdAsync(result.DebitTransactionId);
            var credit = await transactionsRepo.GetByIdAsync(result.CreditTransactionId);

            Assert.NotNull(debit);
            Assert.NotNull(credit);

            Assert.Equal(result.TransferId, debit!.TransferId);
            Assert.Equal(result.TransferId, credit!.TransferId);

            Assert.Equal(TransactionType.Debit, debit.Type);
            Assert.Equal(TransactionType.Credit, credit.Type);

            Assert.Equal(from.Id, debit.AccountId);
            Assert.Equal(to.Id, credit.AccountId);

            Assert.Equal(500m, debit.Amount);
            Assert.Equal(500m, credit.Amount);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrowValidationException_WhenFromAndToAreSame()
        {
            // Arrange
            var accountsRepo = new InMemoryAccountRepository();
            var transactionsRepo = new InMemoryTransactionRepository();

            var acc = new personal_finance.Domain.Entities.Account("Nubank", AccountType.Bank);
            await accountsRepo.AddAsync(acc);

            var handler = new CreateTransferHandler(transactionsRepo, accountsRepo);

            var cmd = new CreateTransferCommand
            {
                FromAccountId = acc.Id,
                ToAccountId = acc.Id,
                Amount = 10m,
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "invalid"
            };

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.HandleAsync(cmd));
        }

        [Fact]
        public async Task HandleAsync_ShouldThrowNotFoundException_WhenFromAccountDoesNotExist()
        {
            // Arrange
            var accountsRepo = new InMemoryAccountRepository();
            var transactionsRepo = new InMemoryTransactionRepository();

            var to = new personal_finance.Domain.Entities.Account("Nubank", AccountType.Bank);
            await accountsRepo.AddAsync(to);

            var handler = new CreateTransferHandler(transactionsRepo, accountsRepo);

            var cmd = new CreateTransferCommand
            {
                FromAccountId = Guid.NewGuid(), // não existe
                ToAccountId = to.Id,
                Amount = 10m,
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Transfer"
            };

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(cmd));
        }

    }
}

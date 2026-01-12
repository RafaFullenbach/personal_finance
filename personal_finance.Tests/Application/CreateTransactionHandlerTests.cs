using personal_finance.Application.Exceptions;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
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
            var accountsRepo = new InMemoryAccountRepository();

            var account = new Account("Wallet", AccountType.Cash);
            await accountsRepo.AddAsync(account);

            var handler = new CreateTransactionHandler(repo, accountsRepo);

            var result = await handler.HandleAsync(new CreateTransactionCommand
            {
                AccountId = account.Id,
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
            var accountsRepo = new InMemoryAccountRepository();

            var account = new Account("Wallet", AccountType.Cash);
            await accountsRepo.AddAsync(account);

            var handler = new CreateTransactionHandler(repo, accountsRepo);

            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await handler.HandleAsync(new CreateTransactionCommand
                {
                    AccountId = account.Id,
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
            var accountsRepo = new InMemoryAccountRepository();

            var account = new Account("Wallet", AccountType.Cash);
            await accountsRepo.AddAsync(account);

            var handler = new CreateTransactionHandler(repo, accountsRepo);

            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await handler.HandleAsync(new CreateTransactionCommand
                {
                    AccountId = account.Id,
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

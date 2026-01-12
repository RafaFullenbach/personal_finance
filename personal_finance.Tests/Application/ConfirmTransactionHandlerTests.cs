using personal_finance.Application.Exceptions;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Domain.Enums;
using personal_finance.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Tests.Application
{
    public class ConfirmTransactionHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldConfirmTransaction_WhenPending()
        {
            var repo = new InMemoryTransactionRepository();
            var accountsRepo = new InMemoryAccountRepository();

            var account = new personal_finance.Domain.Entities.Account("Nubank", AccountType.Bank);
            await accountsRepo.AddAsync(account);

            // cria
            var create = new CreateTransactionHandler(repo, accountsRepo);
            var created = await create.HandleAsync(new CreateTransactionCommand
            {
                AccountId = account.Id,
                Amount = 200m,
                Type = "Credit",
                TransactionDate = DateTime.Today,
                CompetenceYear = 2026,
                CompetenceMonth = 1,
                Description = "Salary"
            });

            // confirma
            var confirm = new ConfirmTransactionHandler(repo);
            var confirmed = await confirm.HandleAsync(new ConfirmTransactionCommand
            {
                TransactionId = created.Id
            });

            Assert.Equal(created.Id, confirmed.TransactionId);
            Assert.Equal("Confirmed", confirmed.Status);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenTransactionNotFound()
        {
            var repo = new InMemoryTransactionRepository();
            var confirm = new ConfirmTransactionHandler(repo);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await confirm.HandleAsync(new ConfirmTransactionCommand
                {
                    TransactionId = Guid.NewGuid()
                });
            });
        }
    }
}

using FluentAssertions;
using Moq;
using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.UseCases.Transactions.UpdateTransaction;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using Xunit;

namespace personal_finance.Tests.Application.Transactions;

public sealed class UpdateTransactionHandlerTests
{
    [Fact]
    public async Task HandleAsync_should_update_pending_transaction_and_save()
    {
        // Arrange
        var account = new Account("Conta", AccountType.Bank);

        var tx = new Transaction(
            200m, TransactionType.Debit, new DateTime(2026, 2, 1), 2026, 2, "Old",
            accountId: account.Id, transferId: null, categoryId: null
        );

        var txRepo = new Mock<ITransactionRepository>();
        txRepo.Setup(r => r.GetByIdAsync(tx.Id)).ReturnsAsync(tx);

        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var categories = new Mock<ICategoryRepository>();

        var handler = new UpdateTransactionHandler(txRepo.Object, accounts.Object, categories.Object);

        var cmd = new UpdateTransactionCommand
        {
            Id = tx.Id,
            Amount = 250m,
            Type = "Debit",
            TransactionDate = new DateTime(2026, 2, 1),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "Updated",
            AccountId = account.Id,
            CategoryId = null
        };

        // Act
        var result = await handler.HandleAsync(cmd);

        // Assert
        result.Id.Should().Be(tx.Id);
        txRepo.Verify(r => r.UpdateAsync(tx), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_should_throw_NotFoundException_when_transaction_not_found()
    {
        var txRepo = new Mock<ITransactionRepository>();
        txRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction?)null);

        var accounts = new Mock<IAccountRepository>();
        var categories = new Mock<ICategoryRepository>();

        var handler = new UpdateTransactionHandler(txRepo.Object, accounts.Object, categories.Object);

        var cmd = new UpdateTransactionCommand
        {
            Id = Guid.NewGuid(),
            Amount = 10m,
            Type = "Debit",
            TransactionDate = new DateTime(2026, 2, 1),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "x",
            AccountId = Guid.NewGuid(),
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        await act.Should().ThrowAsync<NotFoundException>();
        txRepo.Verify(r => r.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_BusinessRuleException_when_transaction_not_pending()
    {
        var account = new Account("Conta", AccountType.Bank);

        var tx = new Transaction(
            10m, TransactionType.Debit, new DateTime(2026, 2, 1), 2026, 2, "x",
            accountId: account.Id, transferId: null, categoryId: null
        );
        tx.Confirm(); // agora não pode atualizar

        var txRepo = new Mock<ITransactionRepository>();
        txRepo.Setup(r => r.GetByIdAsync(tx.Id)).ReturnsAsync(tx);

        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var categories = new Mock<ICategoryRepository>();

        var handler = new UpdateTransactionHandler(txRepo.Object, accounts.Object, categories.Object);

        var cmd = new UpdateTransactionCommand
        {
            Id = tx.Id,
            Amount = 99m,
            Type = "Debit",
            TransactionDate = new DateTime(2026, 2, 1),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "Should fail",
            AccountId = account.Id,
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        await act.Should().ThrowAsync<BusinessRuleException>();
        txRepo.Verify(r => r.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_ValidationException_when_type_is_invalid()
    {
        var txRepo = new Mock<ITransactionRepository>();
        var accounts = new Mock<IAccountRepository>();
        var categories = new Mock<ICategoryRepository>();

        var handler = new UpdateTransactionHandler(txRepo.Object, accounts.Object, categories.Object);

        var cmd = new UpdateTransactionCommand
        {
            Id = Guid.NewGuid(),
            Amount = 10m,
            Type = "INVALID",
            TransactionDate = new DateTime(2026, 2, 1),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "x",
            AccountId = Guid.NewGuid(),
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        var ex = await act.Should().ThrowAsync<ValidationException>();
        ex.Which.Code.Should().Be(ErrorCodes.TransactionInvalidType);
    }
}

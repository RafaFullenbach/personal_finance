using FluentAssertions;
using Moq;
using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Application.Interfaces.CloseMonth;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.UseCases.Transactions.CreateTransaction;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using Xunit;

namespace personal_finance.Tests.Application.Transactions;

public sealed class CreateTransactionHandlerTests
{
    [Fact]
    public async Task HandleAsync_should_create_transaction_when_valid()
    {
        // Arrange
        var account = new Account("Conta", AccountType.Bank);

        var txRepo = new Mock<ITransactionRepository>();
        txRepo.Setup(r => r.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);

        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var closings = new Mock<IMonthClosingRepository>();
        closings.Setup(x => x.GetByPeriodAsync(2026, 2))
        .ReturnsAsync((MonthClosing?)null);

        var categories = new Mock<ICategoryRepository>();

        var handler = new CreateTransactionHandler(txRepo.Object, accounts.Object, closings.Object, categories.Object);

        var cmd = new CreateTransactionCommand
        {
            Amount = 100m,
            Type = "Credit",
            TransactionDate = new DateTime(2026, 2, 5),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "Salary",
            AccountId = account.Id,
            CategoryId = null
        };

        // Act
        var result = await handler.HandleAsync(cmd);

        // Assert
        result.Amount.Should().Be(100m);
        result.Type.Should().Be("Credit");
        result.Status.Should().Be("Pending");
        result.AccountId.Should().Be(account.Id);

        txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_should_throw_ValidationException_when_amount_is_invalid()
    {
        var txRepo = new Mock<ITransactionRepository>();
        var accounts = new Mock<IAccountRepository>();
        var closings = new Mock<IMonthClosingRepository>();
        var categories = new Mock<ICategoryRepository>();

        var handler = new CreateTransactionHandler(txRepo.Object, accounts.Object, closings.Object, categories.Object);

        var cmd = new CreateTransactionCommand
        {
            Amount = 0m,
            Type = "Credit",
            TransactionDate = new DateTime(2026, 2, 5),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "Salary",
            AccountId = Guid.NewGuid(),
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        var ex = await act.Should().ThrowAsync<ValidationException>();
        ex.Which.Code.Should().Be(ErrorCodes.TransactionInvalidAmount);

        txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_ValidationException_when_month_is_closed()
    {
        // Arrange
        var account = new Account("Conta", AccountType.Bank);

        var txRepo = new Mock<ITransactionRepository>();

        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var categories = new Mock<ICategoryRepository>();

        var closings = new Mock<IMonthClosingRepository>();

        closings
            .Setup(r => r.GetByPeriodAsync(2026, 2))
            .ReturnsAsync(new MonthClosing(2026, 2));

        var handler = new CreateTransactionHandler(txRepo.Object, accounts.Object, closings.Object, categories.Object);

        var cmd = new CreateTransactionCommand
        {
            Amount = 10m,
            Type = "Credit",
            TransactionDate = new DateTime(2026, 2, 5),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "x",
            AccountId = account.Id,
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        var ex = await act.Should().ThrowAsync<ValidationException>();
        ex.Which.Code.Should().Be(ErrorCodes.MonthClosed);

        txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_NotFoundException_when_account_not_found()
    {
        var txRepo = new Mock<ITransactionRepository>();
        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Account?)null);

        var closings = new Mock<IMonthClosingRepository>();
        closings.Setup(x => x.GetByPeriodAsync(2026, 2))
        .ReturnsAsync((MonthClosing?)null);

        var categories = new Mock<ICategoryRepository>();

        var handler = new CreateTransactionHandler(txRepo.Object, accounts.Object, closings.Object, categories.Object);

        var cmd = new CreateTransactionCommand
        {
            Amount = 10m,
            Type = "Credit",
            TransactionDate = new DateTime(2026, 2, 5),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "x",
            AccountId = Guid.NewGuid(),
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        await act.Should().ThrowAsync<NotFoundException>();
        txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_BusinessRuleException_when_account_inactive()
    {
        var account = new Account("Conta", AccountType.Bank);
        account.Deactivate(); // se seu domínio tiver esse método; se não tiver, set IsActive conforme permitido

        var txRepo = new Mock<ITransactionRepository>();

        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var closings = new Mock<IMonthClosingRepository>();
        closings.Setup(x => x.GetByPeriodAsync(2026, 2))
        .ReturnsAsync((MonthClosing?)null);

        var categories = new Mock<ICategoryRepository>();

        var handler = new CreateTransactionHandler(txRepo.Object, accounts.Object, closings.Object, categories.Object);

        var cmd = new CreateTransactionCommand
        {
            Amount = 10m,
            Type = "Credit",
            TransactionDate = new DateTime(2026, 2, 5),
            CompetenceYear = 2026,
            CompetenceMonth = 2,
            Description = "x",
            AccountId = account.Id,
            CategoryId = null
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        await act.Should().ThrowAsync<BusinessRuleException>();
        txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }
}

using FluentAssertions;
using Moq;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.UseCases.Transactions.ConfirmTransaction;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using Xunit;

namespace personal_finance.Tests.Application.Transactions;

public sealed class ConfirmTransactionHandlerTests
{
    [Fact]
    public async Task HandleAsync_should_confirm_pending_and_update()
    {
        // Arrange
        var tx = new Transaction(
            amount: 100m,
            type: TransactionType.Credit,
            transactionDate: new DateTime(2026, 2, 5),
            competenceYear: 2026,
            competenceMonth: 2,
            description: "Salary",
            accountId: Guid.NewGuid(),
            transferId: null,
            categoryId: null
        );

        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(tx.Id))
            .ReturnsAsync(tx);

        var handler = new ConfirmTransactionHandler(repo.Object);

        var cmd = new ConfirmTransactionCommand
        {
            TransactionId = tx.Id
        };

        // Act
        var result = await handler.HandleAsync(cmd);

        // Assert
        result.TransactionId.Should().Be(tx.Id);
        result.Status.Should().Be("Confirmed");

        repo.Verify(r => r.UpdateAsync(tx), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_should_throw_NotFoundException_when_transaction_does_not_exist()
    {
        // Arrange
        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Transaction?)null);

        var handler = new ConfirmTransactionHandler(repo.Object);

        var cmd = new ConfirmTransactionCommand
        {
            TransactionId = Guid.NewGuid()
        };

        // Act
        Func<Task> act = () => handler.HandleAsync(cmd);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        repo.Verify(r => r.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_BusinessRuleException_when_transaction_is_not_pending()
    {
        // Arrange
        var tx = new Transaction(
            10m,
            TransactionType.Debit,
            new DateTime(2026, 2, 1),
            2026,
            2,
            "Test",
            accountId: Guid.NewGuid(),
            transferId: null,
            categoryId: null
        );

        tx.Confirm(); // agora não é Pending

        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(tx.Id))
            .ReturnsAsync(tx);

        var handler = new ConfirmTransactionHandler(repo.Object);

        var cmd = new ConfirmTransactionCommand
        {
            TransactionId = tx.Id
        };

        // Act
        Func<Task> act = () => handler.HandleAsync(cmd);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>();
        repo.Verify(r => r.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
    }
}

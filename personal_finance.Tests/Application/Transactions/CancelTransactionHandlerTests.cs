using FluentAssertions;
using Moq;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.UseCases.Transactions.CancelTransaction;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using Xunit;

namespace personal_finance.Tests.Application.Transactions;

public sealed class CancelTransactionHandlerTests
{
    [Fact]
    public async Task HandleAsync_should_cancel_pending_and_update()
    {
        var tx = new Transaction(
            50m, TransactionType.Debit, new DateTime(2026, 2, 10), 2026, 2, "Internet",
            accountId: Guid.NewGuid(), transferId: null, categoryId: null
        );

        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(tx);

        var handler = new CancelTransactionHandler(repo.Object);
        var cmd = new CancelTransactionCommand { TransactionId = Guid.NewGuid() };

        var result = await handler.HandleAsync(cmd);

        result.TransactionId.Should().Be(tx.Id);
        result.Status.Should().Be("Cancelled");
        repo.Verify(r => r.UpdateAsync(tx), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_should_throw_NotFoundException_when_transaction_does_not_exist()
    {
        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction?)null);

        var handler = new CancelTransactionHandler(repo.Object);
        var cmd = new CancelTransactionCommand { TransactionId = Guid.NewGuid() };

        Func<Task> act = () => handler.HandleAsync(cmd);

        await act.Should().ThrowAsync<NotFoundException>();
        repo.Verify(r => r.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_should_throw_BusinessRuleException_when_not_pending()
    {
        var tx = new Transaction(
            10m,
            TransactionType.Credit,
            new DateTime(2026, 2, 1),
            2026,
            2,
            "x",
            accountId: Guid.NewGuid(),
            transferId: null,
            categoryId: null
        );

        tx.Confirm(); 

        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByIdAsync(tx.Id))
            .ReturnsAsync(tx);

        var handler = new CancelTransactionHandler(repo.Object);

        var cmd = new CancelTransactionCommand
        {
            TransactionId = tx.Id
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        await act.Should().ThrowAsync<BusinessRuleException>();
        repo.Verify(r => r.UpdateAsync(It.IsAny<Transaction>()), Times.Never);
    }

}

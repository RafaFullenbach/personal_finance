using FluentAssertions;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;

namespace personal_finance.Tests.Domain.Transactions;

public sealed class TransactionDomainTests
{
    private static Transaction NewPending()
        => new Transaction(
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

    [Fact]
    public void Confirm_should_set_status_confirmed_when_pending()
    {
        var tx = NewPending();

        tx.Confirm();

        tx.Status.Should().Be(TransactionStatus.Confirmed);
    }

    [Fact]
    public void Confirm_should_throw_when_not_pending()
    {
        var tx = NewPending();
        tx.Confirm();

        Action act = () => tx.Confirm();

        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Cancel_should_set_status_cancelled_when_pending()
    {
        var tx = NewPending();

        tx.Cancel();

        tx.Status.Should().Be(TransactionStatus.Cancelled);
    }

    [Fact]
    public void Cancel_should_throw_when_not_pending()
    {
        var tx = NewPending();
        tx.Confirm();

        Action act = () => tx.Cancel();

        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Update_should_update_fields_when_pending()
    {
        var tx = NewPending();
        var newAccountId = Guid.NewGuid();
        var newCategoryId = Guid.NewGuid();

        tx.Update(
            amount: 250m,
            type: TransactionType.Debit,
            transactionDate: new DateTime(2026, 2, 10),
            competenceYear: 2026,
            competenceMonth: 2,
            description: "Updated",
            accountId: newAccountId,
            categoryId: newCategoryId
        );

        tx.Amount.Should().Be(250m);
        tx.Type.Should().Be(TransactionType.Debit);
        tx.Description.Should().Be("Updated");
        tx.AccountId.Should().Be(newAccountId);
        tx.CategoryId.Should().Be(newCategoryId);
    }

    [Fact]
    public void Update_should_throw_InvalidOperationException_when_not_pending()
    {
        var tx = NewPending();
        tx.Confirm();

        Action act = () => tx.Update(
            amount: 200m,
            type: TransactionType.Debit,
            transactionDate: new DateTime(2026, 2, 10),
            competenceYear: 2026,
            competenceMonth: 2,
            description: "x",
            accountId: Guid.NewGuid(),
            categoryId: null
        );

        act.Should().Throw<BusinessRuleException>() ;
    }

    [Fact]
    public void Update_should_throw_ArgumentException_when_amount_invalid()
    {
        var tx = NewPending();

        Action act = () => tx.Update(
            amount: 0m,
            type: TransactionType.Debit,
            transactionDate: new DateTime(2026, 2, 10),
            competenceYear: 2026,
            competenceMonth: 2,
            description: "x",
            accountId: Guid.NewGuid(),
            categoryId: null
        );

        act.Should().Throw<ArgumentException>();
    }
}

using FluentAssertions;
using Moq;
using personal_finance.Application.Interfaces.Recurring;
using personal_finance.Application.UseCases.Recurring.ActivateRecurringTemplate;
using personal_finance.Application.UseCases.Recurring.DeactivateRecurringTemplate;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using Xunit;

namespace personal_finance.Tests.Application.Recurring;

public sealed class ToggleRecurringTemplateHandlerTests
{
    [Fact]
    public async Task Deactivate_should_set_inactive()
    {
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var template = new RecurringTransactionTemplate(
            amount: 100m,
            type: TransactionType.Debit,
            accountId: accountId,
            categoryId: categoryId,
            description: "Assinatura",
            dayOfMonth: 8,
            startDate: new DateTime(2026, 1, 1));

        var repo = new Mock<IRecurringTemplateRepository>();
        repo.Setup(r => r.GetByIdAsync(template.Id)).ReturnsAsync(template);

        var handler = new DeactivateRecurringTemplateHandler(repo.Object);

        var result = await handler.HandleAsync(new DeactivateRecurringTemplateCommand { Id = template.Id });

        result.IsActive.Should().BeFalse();
        repo.Verify(r => r.UpdateAsync(template), Times.Once);
    }

    [Fact]
    public async Task Activate_should_set_active()
    {
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var template = new RecurringTransactionTemplate(
            amount: 100m,
            type: TransactionType.Debit,
            accountId: accountId,
            categoryId: categoryId,
            description: "Assinatura",
            dayOfMonth: 8,
            startDate: new DateTime(2026, 1, 1));

        template.Deactivate();

        var repo = new Mock<IRecurringTemplateRepository>();
        repo.Setup(r => r.GetByIdAsync(template.Id)).ReturnsAsync(template);

        var handler = new ActivateRecurringTemplateHandler(repo.Object);

        var result = await handler.HandleAsync(new ActivateRecurringTemplateCommand { Id = template.Id });

        result.IsActive.Should().BeTrue();
        repo.Verify(r => r.UpdateAsync(template), Times.Once);
    }
}

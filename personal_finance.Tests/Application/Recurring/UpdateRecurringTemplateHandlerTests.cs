using FluentAssertions;
using Moq;
using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Application.Interfaces.Recurring;
using personal_finance.Application.UseCases.Recurring.UpdateRecurringTemplate;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using Xunit;

namespace personal_finance.Tests.Application.Recurring;

public sealed class UpdateRecurringTemplateHandlerTests
{
    [Fact]
    public async Task HandleAsync_should_update_template_and_save()
    {
        var account = new Account("Conta", AccountType.Bank);
        var category = new Category("Moradia", CategoryType.Expense);

        var template = new RecurringTransactionTemplate(
            amount: 100m,
            type: TransactionType.Debit,
            accountId: account.Id,
            categoryId: category.Id,
            description: "Aluguel",
            dayOfMonth: 5,
            startDate: new DateTime(2026, 1, 1));

        var templates = new Mock<IRecurringTemplateRepository>();
        templates.Setup(r => r.GetByIdAsync(template.Id)).ReturnsAsync(template);

        var accounts = new Mock<IAccountRepository>();
        accounts.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var categories = new Mock<ICategoryRepository>();
        categories.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var handler = new UpdateRecurringTemplateHandler(templates.Object, accounts.Object, categories.Object);

        var cmd = new UpdateRecurringTemplateCommand
        {
            Id = template.Id,
            Amount = 150m,
            Type = "Debit",
            AccountId = account.Id,
            CategoryId = category.Id,
            Description = "Aluguel Atualizado",
            DayOfMonth = 10,
            CompetenceOffsetMonths = 1,
            StartDate = new DateTime(2026, 2, 1),
            EndDate = new DateTime(2026, 12, 31)
        };

        var result = await handler.HandleAsync(cmd);

        result.Id.Should().Be(template.Id);
        template.Amount.Should().Be(150m);
        template.DayOfMonth.Should().Be(10);
        template.CompetenceOffsetMonths.Should().Be(1);
        templates.Verify(r => r.UpdateAsync(template), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_should_throw_when_type_is_invalid()
    {
        var templates = new Mock<IRecurringTemplateRepository>();
        var accounts = new Mock<IAccountRepository>();
        var categories = new Mock<ICategoryRepository>();

        var handler = new UpdateRecurringTemplateHandler(templates.Object, accounts.Object, categories.Object);

        var cmd = new UpdateRecurringTemplateCommand
        {
            Id = Guid.NewGuid(),
            Amount = 100m,
            Type = "X",
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            Description = "Teste",
            DayOfMonth = 5,
            StartDate = new DateTime(2026, 1, 1)
        };

        Func<Task> act = () => handler.HandleAsync(cmd);

        var ex = await act.Should().ThrowAsync<ValidationException>();
        ex.Which.Code.Should().Be(ErrorCodes.RecurringInvalidType);
    }
}

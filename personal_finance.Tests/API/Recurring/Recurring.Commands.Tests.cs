using FluentAssertions;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Infrastructure.Persistence;
using personal_finance.Tests.Infrastructure.TestFactory;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace personal_finance.Tests.API.Recurring;

public sealed class RecurringCommandsTests
{
    private sealed class RecurringTemplateResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
    }

    private sealed class RecurringTemplateListItem
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = default!;
    }

    private static async Task<Guid> SeedCategoryAsync(ApiFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var category = new Category("Categoria Teste", CategoryType.Expense);
        db.Categories.Add(category);
        await db.SaveChangesAsync();

        return category.Id;
    }

    private static async Task<RecurringTemplateResponse> CreateRecurringAsync(HttpClient client, Guid accountId, Guid categoryId)
    {
        var payload = new
        {
            amount = 100m,
            type = "Debit",
            accountId,
            categoryId,
            description = "Aluguel",
            dayOfMonth = 5,
            competenceOffsetMonths = 0,
            startDate = "2026-01-01T00:00:00",
            endDate = (string?)null
        };

        var response = await client.PostAsJsonAsync("/recurring/templates", payload);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<RecurringTemplateResponse>();
        created.Should().NotBeNull();
        return created!;
    }

    [Fact]
    public async Task PUT_recurring_templates_should_update_template()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var categoryId = await SeedCategoryAsync(factory);
        var created = await CreateRecurringAsync(client, accountId, categoryId);

        var updatePayload = new
        {
            amount = 140m,
            type = "Debit",
            accountId,
            categoryId,
            description = "Aluguel atualizado",
            dayOfMonth = 10,
            competenceOffsetMonths = 1,
            startDate = "2026-02-01T00:00:00",
            endDate = "2026-12-31T00:00:00"
        };

        var updateResp = await client.PutAsJsonAsync($"/recurring/templates/{created.Id}", updatePayload);
        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task POST_recurring_templates_activate_and_deactivate_should_toggle_status()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var categoryId = await SeedCategoryAsync(factory);
        var created = await CreateRecurringAsync(client, accountId, categoryId);

        var deactivateResp = await client.PostAsync($"/recurring/templates/{created.Id}/deactivate", null);
        deactivateResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var deactivated = await deactivateResp.Content.ReadFromJsonAsync<RecurringTemplateResponse>();
        deactivated.Should().NotBeNull();
        deactivated!.IsActive.Should().BeFalse();

        var activateResp = await client.PostAsync($"/recurring/templates/{created.Id}/activate", null);
        activateResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var activated = await activateResp.Content.ReadFromJsonAsync<RecurringTemplateResponse>();
        activated.Should().NotBeNull();
        activated!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task POST_recurring_templates_deactivate_should_hide_from_default_query()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var categoryId = await SeedCategoryAsync(factory);
        var created = await CreateRecurringAsync(client, accountId, categoryId);

        var deactivateResp = await client.PostAsync($"/recurring/templates/{created.Id}/deactivate", null);
        deactivateResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultListResp = await client.GetAsync("/recurring/templates");
        defaultListResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var defaultList = await defaultListResp.Content.ReadFromJsonAsync<List<RecurringTemplateListItem>>();
        defaultList.Should().NotBeNull();
        defaultList!.Any(x => x.Id == created.Id).Should().BeFalse();

        var includeInactiveResp = await client.GetAsync("/recurring/templates?includeInactive=true");
        includeInactiveResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var includeInactiveList = await includeInactiveResp.Content.ReadFromJsonAsync<List<RecurringTemplateListItem>>();
        includeInactiveList.Should().NotBeNull();
        includeInactiveList!.Any(x => x.Id == created.Id && !x.IsActive).Should().BeTrue();
    }
}

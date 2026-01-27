using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using personal_finance.Tests.Infrastructure.TestFactory;
using Xunit;

namespace personal_finance.Tests.API.Transactions;

public sealed class TransactionQueriesTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;

    public TransactionQueriesTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_transactions_should_return_created_item()
    {
        // Arrange
        var accountId = await _factory.SeedAccountAsync();

        var createPayload = new
        {
            amount = 120.5m,
            type = "Debit",
            transactionDate = "2026-02-10T00:00:00",
            competenceYear = 2026,
            competenceMonth = 2,
            description = "Rent",
            accountId = accountId
        };

        var createResp = await _client.PostAsJsonAsync("/transactions", createPayload);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act
        var resp = await _client.GetAsync("/transactions?page=1&pageSize=20&description=Rent");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await resp.Content.ReadFromJsonAsync<PagedResult<TransactionListItem>>();
        body.Should().NotBeNull();
        body!.TotalItems.Should().BeGreaterThanOrEqualTo(1);
        body.Items.Should().Contain(x => x.Description == "Rent" && x.AccountId == accountId);
    }

    private sealed class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    private sealed class TransactionListItem
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public int CompetenceYear { get; set; }
        public int CompetenceMonth { get; set; }
        public string Description { get; set; } = default!;
        public Guid AccountId { get; set; }
    }
}

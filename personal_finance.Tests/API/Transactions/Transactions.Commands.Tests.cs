using FluentAssertions;
using personal_finance.Tests.Infrastructure.Http;
using personal_finance.Tests.Infrastructure.TestFactory;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Channels;
using Xunit.Sdk;

namespace personal_finance.Tests.API.Transactions;

public sealed class TransactionsCommandsTests
{
    private sealed class CreateTransactionResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
    }

    private sealed class ConfirmTransactionResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = default!;
    }

    private sealed class CancelTransactionResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = default!;
    }

    private static object CreatePayload(
        Guid accountId,
        decimal amount,
        string type,
        string transactionDate,
        int competenceYear,
        int competenceMonth,
        string description
    ) => new
    {
        amount,
        type,
        transactionDate,
        competenceYear,
        competenceMonth,
        description,
        accountId
    };

    private static async Task<CreateTransactionResponse> CreateTransactionAsync(
        HttpClient client,
        Guid accountId,
        decimal amount = 100m,
        string type = "Credit",
        string transactionDate = "2026-02-05T00:00:00",
        int competenceYear = 2026,
        int competenceMonth = 2,
        string description = "Salary")
    {
        var payload = CreatePayload(
            accountId,
            amount,
            type,
            transactionDate,
            competenceYear,
            competenceMonth,
            description
        );

        var response = await client.PostAsJsonAsync("/transactions", payload);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<CreateTransactionResponse>();
        body.Should().NotBeNull();
        return body!;
    }

    [Fact]
    public async Task POST_transactions_should_create_and_return_201()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();

        var created = await CreateTransactionAsync(
            client,
            accountId,
            amount: 100m,
            type: "Credit",
            description: "Salary"
        );

        created.Amount.Should().Be(100m);
        created.Type.Should().Be("Credit");
        created.Status.Should().Be("Pending");
        created.AccountId.Should().Be(accountId);
    }

    [Fact]
    public async Task POST_transactions_confirm_should_confirm_pending_transaction()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var created = await CreateTransactionAsync(client, accountId);

        var confirmResp = await client.PostAsync($"/transactions/{created.Id}/confirm", null);
        confirmResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await confirmResp.Content.ReadFromJsonAsync<ConfirmTransactionResponse>();
        body.Should().NotBeNull();
        body!.Status.Should().Be("Confirmed");
    }

    [Fact]
    public async Task POST_transactions_cancel_should_cancel_pending_transaction()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var created = await CreateTransactionAsync(
            client,
            accountId,
            amount: 50m,
            type: "Debit",
            transactionDate: "2026-02-10T00:00:00",
            description: "Internet"
        );

        var cancelResp = await client.PostAsync($"/transactions/{created.Id}/cancel", null);
        cancelResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await cancelResp.Content.ReadFromJsonAsync<CancelTransactionResponse>();
        body.Should().NotBeNull();
        body!.Status.Should().Be("Cancelled");
    }

    [Fact]
    public async Task PUT_transactions_should_update_pending_transaction()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var created = await CreateTransactionAsync(
            client,
            accountId,
            amount: 200m,
            type: "Debit",
            transactionDate: "2026-02-01T00:00:00",
            description: "Old description"
        );

        var updatePayload = CreatePayload(
            accountId,
            amount: 250m,
            type: "Debit",
            transactionDate: "2026-02-01T00:00:00",
            competenceYear: 2026,
            competenceMonth: 2,
            description: "Updated description"
        );

        var updateResp = await client.PutAsJsonAsync($"/transactions/{created.Id}", updatePayload);
        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PUT_transactions_should_fail_if_transaction_is_confirmed()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var created = await CreateTransactionAsync(
            client,
            accountId,
            amount: 100m,
            type: "Debit",
            transactionDate: "2026-02-01T00:00:00",
            description: "Test"
        );

        var confirmResp = await client.PostAsync($"/transactions/{created.Id}/confirm", null);
        confirmResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatePayload = CreatePayload(
            accountId,
            amount: 999m,
            type: "Debit",
            transactionDate: "2026-02-01T00:00:00",
            competenceYear: 2026,
            competenceMonth: 2,
            description: "Should fail"
        );

        var updateResp = await client.PutAsJsonAsync($"/transactions/{created.Id}", updatePayload);

        await updateResp.AssertProblemAsync(
            expectedStatus: HttpStatusCode.Conflict,
            expectedDetail: "Apenas transações pendentes podem ser editadas.",
            expectCode: false,
            expectedTitle: "Conflict"
        );
    }

    [Fact]
    public async Task POST_transactions_when_amount_is_zero_should_return_400_with_code()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();

        var payload = CreatePayload(
            accountId,
            amount: 0m,
            type: "Credit",
            transactionDate: "2026-02-05T00:00:00",
            competenceYear: 2026,
            competenceMonth: 2,
            description: "Salary"
        );

        var resp = await client.PostAsJsonAsync("/transactions", payload);

        await resp.AssertProblemAsync(
            expectedStatus: HttpStatusCode.BadRequest,
            expectedCode: "TRANSACTION_INVALID_AMOUNT",
            expectedTitle: "BadRequest"
        );
    }

    [Fact]
    public async Task POST_transactions_confirm_should_return_409_if_transaction_is_not_pending()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var created = await CreateTransactionAsync(client, accountId);

        var confirm1 = await client.PostAsync($"/transactions/{created.Id}/confirm", null);
        confirm1.StatusCode.Should().Be(HttpStatusCode.OK);

        var confirm2 = await client.PostAsync($"/transactions/{created.Id}/confirm", null);

        await confirm2.AssertProblemAsync(
            expectedStatus: HttpStatusCode.Conflict,
            expectedTitle: "Conflict",
            expectedDetail: "Apenas transações pendentes podem ser confirmadas.",
            expectCode: false
        );
    }

    [Fact]
    public async Task POST_transactions_cancel_should_return_409_if_transaction_is_not_pending()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();
        var created = await CreateTransactionAsync(client, accountId);

        var confirm = await client.PostAsync($"/transactions/{created.Id}/confirm", null);
        confirm.StatusCode.Should().Be(HttpStatusCode.OK);

        var cancel = await client.PostAsync($"/transactions/{created.Id}/cancel", null);

        await cancel.AssertProblemAsync(
            expectedStatus: HttpStatusCode.Conflict,
            expectedTitle: "Conflict",
            expectedDetail: "Apenas transações pendentes podem ser canceladas.",
            expectCode: false
        );
    }

    [Fact]
    public async Task PUT_transactions_should_return_404_if_transaction_does_not_exist()
    {
        await using var factory = new ApiFactory();
        using var client = factory.CreateClient();

        var accountId = await factory.SeedAccountAsync();

        var updatePayload = CreatePayload(
            accountId,
            amount: 10m,
            type: "Debit",
            transactionDate: "2026-02-01T00:00:00",
            competenceYear: 2026,
            competenceMonth: 2,
            description: "Does not matter"
        );

        var nonExistingId = Guid.NewGuid();
        var resp = await client.PutAsJsonAsync($"/transactions/{nonExistingId}", updatePayload);

        var resourceName = "Lançamento";

        await resp.AssertProblemAsync(
              expectedStatus: HttpStatusCode.NotFound,
              expectedTitle: "NotFound",
              expectedDetail: $"{resourceName} '{nonExistingId}' não foi encontrado.",
              expectCode: false
          );
    }
}

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace personal_finance.Tests.Infrastructure.Http;

public static class HttpResponseMessageExtensions
{
    public static async Task<ProblemDetailsResponse> ReadProblemAsync(this HttpResponseMessage response)
    {
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull("a resposta deveria retornar um ProblemDetails JSON padronizado");
        return problem!;
    }

    public static async Task AssertProblemAsync(
        this HttpResponseMessage response,
        HttpStatusCode expectedStatus,
        string expectedTitle,
        string? expectedCode = null,
        string? expectedDetail = null,
        bool expectCode = true
    )
    {
        response.StatusCode.Should().Be(expectedStatus);

        var problem = await response.ReadProblemAsync();

        problem.Status.Should().Be((int)expectedStatus);
        problem.Title.Should().Be(expectedTitle);

        if (expectedDetail is not null)
            problem.Detail.Should().Be(expectedDetail);

        if (expectCode)
        {
            problem.Code.Should().NotBeNull("era esperado que o erro tivesse code");
            problem.Code.Should().Be(expectedCode);
        }
        else
        {
            problem.Code.Should().BeNull("não era esperado code nesse tipo de erro");
        }
    }
}

namespace personal_finance.Tests.Infrastructure.Http;

public sealed class ProblemDetailsResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? Detail { get; set; }

    // BusinessRuleException retorna "code": null
    public string? Code { get; set; }
}

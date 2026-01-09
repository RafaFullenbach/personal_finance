using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Reports;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("reports")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class ReportsQueriesController : ControllerBase
    {
        private readonly GetMonthlySummaryHandler _handler;

        public ReportsQueriesController(GetMonthlySummaryHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("monthly-summary")]
        public async Task<IActionResult> GetMonthlySummary(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var result = await _handler.HandleAsync(new GetMonthlySummaryQuery
            {
                Year = year,
                Month = month
            });

            return Ok(result);
        }
    }
}

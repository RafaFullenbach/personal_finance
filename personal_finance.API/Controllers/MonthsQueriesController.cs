using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Interfaces;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("months")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class MonthsQueriesController : ControllerBase
    {
        [HttpGet("{year:int}/{month:int}/status")]
        public async Task<IActionResult> Status(
            [FromRoute] int year,
            [FromRoute] int month,
            [FromServices] IMonthClosingRepository repo)
        {
            var closing = await repo.GetByPeriodAsync(year, month);

            if (closing is null)
                return Ok(new { year, month, isClosed = false });

            return Ok(new { year, month, isClosed = true, closedAt = closing.ClosedAt });
        }
    }
}

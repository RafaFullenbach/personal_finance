using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.UseCases.UpsertBudget;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("budgets")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class BudgetsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upsert(
            [FromBody] UpsertBudgetCommand command,
            [FromServices] UpsertBudgetHandler handler)
        {
            var result = await handler.HandleAsync(command);
            return Ok(result);
        }
    }

    [ApiController]
    [Route("budgets")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class BudgetsQueryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetByMonth(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromServices] GetBudgetsByMonthHandler handler)
        {
            var result = await handler.HandleAsync(year, month);
            return Ok(result);
        }
    }
}

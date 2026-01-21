using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Budgets;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("budgets")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class BudgetsQueriesController : ControllerBase
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
          [FromRoute] Guid id,
          [FromServices] GetBudgetByIdHandler handler)
        {
            var result = await handler.HandleAsync(new GetBudgetByIdQuery { Id = id });
            return Ok(result);
        }
    }
}

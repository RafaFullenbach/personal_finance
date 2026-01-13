using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.UseCases.UpsertBudget;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("budgets")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class BudgetsCommandsController : ControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> Upsert(
         [FromBody] UpsertBudgetCommand command,
         [FromServices] UpsertBudgetHandler handler)
        {
            var result = await handler.HandleAsync(command);
            return Ok(result); // retorna Created/Updated no Action
        }
    }
}

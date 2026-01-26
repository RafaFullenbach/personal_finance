using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.UseCases.Budgets.ActivateBudget;
using personal_finance.Application.UseCases.Budgets.DeactivateBudget;
using personal_finance.Application.UseCases.Budgets.UpsertBudget;

namespace personal_finance.API.Controllers.Budgets
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
            return Ok(result);
        }

        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(
            [FromRoute] Guid id,
            [FromServices] DeactivateBudgetHandler handler)
        {
            var result = await handler.HandleAsync(new DeactivateBudgetCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> Activate(
            [FromRoute] Guid id,
            [FromServices] ActivateBudgetHandler handler)
        {
            var result = await handler.HandleAsync(new ActivateBudgetCommand { Id = id });
            return Ok(result);
        }
    }
}

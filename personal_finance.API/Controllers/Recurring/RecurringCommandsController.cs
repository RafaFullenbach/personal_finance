using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.Recurring.ActivateRecurringTemplate;
using personal_finance.Application.UseCases.Recurring.CreateRecurringTemplate;
using personal_finance.Application.UseCases.Recurring.DeactivateRecurringTemplate;
using personal_finance.Application.UseCases.Recurring.GenerateRecurringTransactions;
using personal_finance.Application.UseCases.Recurring.UpdateRecurringTemplate;

namespace personal_finance.API.Controllers.Recurring
{
    [ApiController]
    [Route("recurring")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class RecurringCommandsController : ControllerBase
    {
        [HttpPost("templates")]
        public async Task<IActionResult> CreateTemplate(
            [FromBody] CreateRecurringTemplateCommand command,
            [FromServices] CreateRecurringTemplateHandler handler)
        {
            var result = await handler.HandleAsync(command);
            return Created($"/recurring/templates/{result.Id}", result);
        }

        [HttpPut("templates/{id:guid}")]
        public async Task<IActionResult> UpdateTemplate(
            [FromRoute] Guid id,
            [FromBody] UpdateRecurringTemplateCommand command,
            [FromServices] UpdateRecurringTemplateHandler handler)
        {
            var result = await handler.HandleAsync(new UpdateRecurringTemplateCommand
            {
                Id = id,
                Amount = command.Amount,
                Type = command.Type,
                AccountId = command.AccountId,
                CategoryId = command.CategoryId,
                Description = command.Description,
                DayOfMonth = command.DayOfMonth,
                CompetenceOffsetMonths = command.CompetenceOffsetMonths,
                StartDate = command.StartDate,
                EndDate = command.EndDate
            });

            return Ok(result);
        }

        [HttpPost("templates/{id:guid}/deactivate")]
        public async Task<IActionResult> DeactivateTemplate(
            [FromRoute] Guid id,
            [FromServices] DeactivateRecurringTemplateHandler handler)
        {
            var result = await handler.HandleAsync(new DeactivateRecurringTemplateCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("templates/{id:guid}/activate")]
        public async Task<IActionResult> ActivateTemplate(
            [FromRoute] Guid id,
            [FromServices] ActivateRecurringTemplateHandler handler)
        {
            var result = await handler.HandleAsync(new ActivateRecurringTemplateCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromServices] GenerateRecurringTransactionsHandler handler)
        {
            var result = await handler.HandleAsync(new GenerateRecurringTransactionsCommand
            {
                Year = year,
                Month = month
            });

            return Ok(result);
        }
    }
}


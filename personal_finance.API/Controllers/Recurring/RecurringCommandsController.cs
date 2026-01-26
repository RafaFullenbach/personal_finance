using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.Recurring.CreateRecurringTemplate;
using personal_finance.Application.UseCases.Recurring.GenerateRecurringTransactions;

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

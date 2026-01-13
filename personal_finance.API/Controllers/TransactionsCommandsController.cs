using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.CancelTransaction;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateTransaction;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("transactions")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class TransactionsCommandsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(
             [FromBody] CreateTransactionCommand command,
             [FromServices] CreateTransactionHandler handler)
        {
            var result = await handler.HandleAsync(command);
            return Created($"/transactions/{result.Id}", result);
        }

        [HttpPost("{id:guid}/confirm")]
        public async Task<IActionResult> Confirm(
           [FromRoute] Guid id,
           [FromServices] ConfirmTransactionHandler handler)
        {
            var result = await handler.HandleAsync(new ConfirmTransactionCommand
            {
                TransactionId = id
            });

            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(
            [FromRoute] Guid id,
            [FromServices] CancelTransactionHandler handler)
        {
            var result = await handler.HandleAsync(new CancelTransactionCommand
            {
                TransactionId = id
            });

            return Ok(result);
        }
    }
}

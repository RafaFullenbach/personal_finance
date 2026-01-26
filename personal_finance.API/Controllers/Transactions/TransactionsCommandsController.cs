using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.Transactions.CancelTransaction;
using personal_finance.Application.UseCases.Transactions.ConfirmTransaction;
using personal_finance.Application.UseCases.Transactions.CreateTransaction;
using personal_finance.Application.UseCases.Transactions.UpdateTransaction;

namespace personal_finance.API.Controllers.Transactions
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateTransactionCommand command,
            [FromServices] UpdateTransactionHandler handler)
        {
            var result = await handler.HandleAsync(new UpdateTransactionCommand
            {
                Id = id,
                Amount = command.Amount,
                Type = command.Type,
                TransactionDate = command.TransactionDate,
                CompetenceYear = command.CompetenceYear,
                CompetenceMonth = command.CompetenceMonth,
                Description = command.Description,
                AccountId = command.AccountId,
                CategoryId = command.CategoryId
            });

            return Ok(result);
        }

    }
}

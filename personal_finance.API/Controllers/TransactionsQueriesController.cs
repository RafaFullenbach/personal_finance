using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Transactions;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("transactions")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class TransactionsQueriesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(
             [FromQuery] int? year,
             [FromQuery] int? month,
             [FromQuery] string? type,
             [FromQuery] string? status,
             [FromServices] GetAllTransactionsHandler handler)
        {
            var result = await handler.HandleAsync(new GetTransactionsQuery
            {
                Year = year,
                Month = month,
                Type = type,
                Status = status
            });

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] GetTransactionByIdHandler handler)
        {
            var result = await handler.HandleAsync(new GetTransactionByIdQuery
            {
                Id = id
            });

            return Ok(result);
        }
    }
}

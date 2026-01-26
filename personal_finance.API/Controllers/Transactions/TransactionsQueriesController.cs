using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Transactions;

namespace personal_finance.API.Controllers.Transactions
{
    [ApiController]
    [Route("transactions")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class TransactionsQueriesController : ControllerBase
    {
        private readonly GetAllTransactionsHandler _handler;

        public TransactionsQueriesController(GetAllTransactionsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? year,
            [FromQuery] int? month,
            [FromQuery] string? type,
            [FromQuery] string? status,
            [FromQuery] string? description,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "transactionDate",
            [FromQuery] string? order = "desc",
            [FromQuery] bool includeTransfers = true
            )

        {
            var result = await _handler.HandleAsync(new GetTransactionsQuery
            {
                Year = year,
                Month = month,
                Type = type,
                Status = status,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                Order = order,
                IncludeTransfers = includeTransfers,
                Description = description
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

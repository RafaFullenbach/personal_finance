using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Accounts;
using personal_finance.Application.Queries.Transactions;

namespace personal_finance.API.Controllers.Accounts
{
    [ApiController]
    [Route("accounts")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class AccountsQueriesController : ControllerBase
    {
        private readonly GetAllAccountsHandler _handler;

        public AccountsQueriesController(GetAllAccountsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _handler.HandleAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
          [FromRoute] Guid id,
          [FromServices] GetAccountByIdHandler handler)
        {
            var result = await handler.HandleAsync(new GetAccountByIdQuery
            {
                Id = id
            });

            return Ok(result);
        }
    }
}

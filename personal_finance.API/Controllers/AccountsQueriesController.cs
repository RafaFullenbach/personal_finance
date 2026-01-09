using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Accounts;

namespace personal_finance.API.Controllers
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
    }
}

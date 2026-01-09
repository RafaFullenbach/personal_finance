using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.CreateAccount;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("accounts")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class AccountsController : ControllerBase
    {
        private readonly CreateAccountHandler _handler;

        public AccountsController(CreateAccountHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
        {
            var result = await _handler.HandleAsync(command);
            return Created($"/accounts/{result.Id}", result);
        }
    }
}

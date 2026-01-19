using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.ActivateAccount;
using personal_finance.Application.UseCases.CreateAccount;
using personal_finance.Application.UseCases.DeactivateAccount;
using personal_finance.Application.UseCases.UpdateAccount;

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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateAccountCommand command,
            [FromServices] UpdateAccountHandler handler)
        {
            command.Id = id;
            var result = await handler.HandleAsync(command);
            return Ok(result);
        }

        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(
             [FromRoute] Guid id,
             [FromServices] DeactivateAccountHandler handler)
        {
            var result = await handler.HandleAsync(new DeactivateAccountCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> Activate(
            [FromRoute] Guid id,
            [FromServices] ActivateAccountHandler handler)
        {
            var result = await handler.HandleAsync(new ActivateAccountCommand { Id = id });
            return Ok(result);
        }
    }
}

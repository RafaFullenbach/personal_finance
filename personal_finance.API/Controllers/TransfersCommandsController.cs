using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.CreateTransfer;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("transfers")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class TransfersCommandsController : ControllerBase
    {
        private readonly CreateTransferHandler _handler;

        public TransfersCommandsController(CreateTransferHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransferCommand command)
        {
            var result = await _handler.HandleAsync(command);
            return Created($"/transfers/{result.TransferId}", result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.UseCases.CloseMonth;

namespace personal_finance.API.Controllers.CloseMonths
{
    [ApiController]
    [Route("months")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class MonthsCommandsController : ControllerBase
    {
        [HttpPost("{year:int}/{month:int}/close")]
        public async Task<IActionResult> Close(
            [FromRoute] int year,
            [FromRoute] int month,
            [FromServices] CloseMonthHandler handler,
            [FromQuery] bool autoConfirmPending = true
            )
        {
            var result = await handler.HandleAsync(new CloseMonthCommand
            {
                Year = year,
                Month = month,
                AutoConfirmPending = autoConfirmPending
            });

            return Ok(result);
        }
    }
}

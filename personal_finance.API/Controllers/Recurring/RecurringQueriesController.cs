using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Recurring;

namespace personal_finance.API.Controllers.Recurring
{
    [ApiController]
    [Route("recurring/templates")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class RecurringQueriesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool includeInactive,
            [FromServices] GetAllRecurringTemplatesHandler handler)
        {
            var result = await handler.HandleAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] GetRecurringTemplateByIdHandler handler)
        {
            var result = await handler.HandleAsync(new GetRecurringTemplateByIdQuery
            {
                Id = id
            });

            return Ok(result);
        }
    }
}

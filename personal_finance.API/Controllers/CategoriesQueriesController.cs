using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Categories;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("categories")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class CategoriesQueryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool includeInactive,
            [FromServices] GetAllCategoriesHandler handler)
        {
            var result = await handler.HandleAsync(includeInactive);
            return Ok(result);
        }
    }
}

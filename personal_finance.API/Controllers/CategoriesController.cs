using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Categories;
using personal_finance.Application.UseCases.CreateCategory;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("categories")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryCommand command,
            [FromServices] CreateCategoryHandler handler)
        {
            var result = await handler.HandleAsync(command);
            return Created($"/categories/{result.Id}", result);
        }
    }

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

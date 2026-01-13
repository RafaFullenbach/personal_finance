using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Categories;
using personal_finance.Application.UseCases.CreateCategory;

namespace personal_finance.API.Controllers
{
    [ApiController]
    [Route("categories")]
    [ApiExplorerSettings(GroupName = "Commands")]
    public class CategoriesCommandsController : ControllerBase
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
}

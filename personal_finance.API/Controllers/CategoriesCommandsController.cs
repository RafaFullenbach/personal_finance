using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Categories;
using personal_finance.Application.UseCases.ActivateCategory;
using personal_finance.Application.UseCases.CreateCategory;
using personal_finance.Application.UseCases.DeactivateCategory;
using personal_finance.Application.UseCases.UpdateCategory;

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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateCategoryCommand command,
            [FromServices] UpdateCategoryHandler handler)
        {
            command.Id = id;
            var result = await handler.HandleAsync(command);
            return Ok(result);
        }

        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(
            [FromRoute] Guid id,
            [FromServices] DeactivateCategoryHandler handler)
        {
            var result = await handler.HandleAsync(new DeactivateCategoryCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> Activate(
            [FromRoute] Guid id,
            [FromServices] ActivateCategoryHandler handler)
        {
            var result = await handler.HandleAsync(new ActivateCategoryCommand { Id = id });
            return Ok(result);
        }
    }
}

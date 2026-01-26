using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Categories.ActivateCategory
{
    public sealed class ActivateCategoryHandler
    {
        private readonly ICategoryRepository _categories;

        public ActivateCategoryHandler(ICategoryRepository categories)
        {
            _categories = categories;
        }

        public async Task<ActivateCategoryResult> HandleAsync(ActivateCategoryCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.CategoryInvalidId);

            var category = await _categories.GetByIdAsync(command.Id);
            if (category is null)
                throw NotFoundException.For("Categoria", command.Id);

            category.Activate();
            await _categories.UpdateAsync(category);

            return new ActivateCategoryResult
            {
                Id = category.Id,
                IsActive = category.IsActive
            };
        }
    }
}

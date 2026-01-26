using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Categories.DeactivateCategory
{
    public sealed class DeactivateCategoryHandler
    {
        private readonly ICategoryRepository _categories;

        public DeactivateCategoryHandler(ICategoryRepository categories)
        {
            _categories = categories;
        }

        public async Task<DeactivateCategoryResult> HandleAsync(DeactivateCategoryCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.CategoryInvalidId);

            var category = await _categories.GetByIdAsync(command.Id);
            if (category is null)
                throw NotFoundException.For("Categoria", command.Id);

            category.Deactivate();
            await _categories.UpdateAsync(category);

            return new DeactivateCategoryResult
            {
                Id = category.Id,
                IsActive = category.IsActive
            };
        }
    }
}

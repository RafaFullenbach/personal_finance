using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.DeactivateCategory
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
                throw ValidationException.Invalid("Id is required.", ErrorCodes.CategoryInvalidId);

            var category = await _categories.GetByIdAsync(command.Id);
            if (category is null)
                throw NotFoundException.For("Category", command.Id);

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

using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.ActivateCategory
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
                throw ValidationException.Invalid("Id is required.", ErrorCodes.CategoryInvalidId);

            var category = await _categories.GetByIdAsync(command.Id);
            if (category is null)
                throw NotFoundException.For("Category", command.Id);

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

using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;

namespace personal_finance.Application.UseCases.UpdateCategory
{
    public sealed class UpdateCategoryHandler
    {
        private readonly ICategoryRepository _categories;
        private readonly ICategoryUsageQueryRepository _usage;

        public UpdateCategoryHandler(ICategoryRepository categories, ICategoryUsageQueryRepository usage)
        {
            _categories = categories;
            _usage = usage;
        }

        public async Task<UpdateCategoryResult> HandleAsync(UpdateCategoryCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id is required.", ErrorCodes.CategoryInvalidId);

            if (string.IsNullOrWhiteSpace(command.Name))
                throw ValidationException.Invalid("Name is required.", ErrorCodes.CategoryInvalidName);

            if (!Enum.TryParse<CategoryType>(command.Type, true, out var newType))
                throw ValidationException.Invalid($"Category type '{command.Type}' is invalid.", ErrorCodes.CategoryInvalidType);

            var category = await _categories.GetByIdAsync(command.Id);
            if (category is null)
                throw NotFoundException.For("Category", command.Id);

            // ✅ Regra: só muda Type se não tiver uso
            if (category.Type != newType)
            {
                var used =
                    await _usage.HasTransactionsAsync(category.Id) ||
                    await _usage.HasBudgetsAsync(category.Id) ||
                    await _usage.HasRecurringTemplatesAsync(category.Id);

                if (used)
                    throw new BusinessRuleException("Category type cannot be changed because it is already in use.");
            }

            try
            {
                category.Rename(command.Name);
                category.ChangeType(newType);

                if (command.IsActive) category.Activate();
                else category.Deactivate();
            }
            catch (ArgumentException ex)
            {
                throw ValidationException.Invalid(ex.Message, ErrorCodes.CategoryInvalidName);
            }

            await _categories.UpdateAsync(category);

            return new UpdateCategoryResult
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type.ToString(),
                IsActive = category.IsActive
            };
        }
    }
}

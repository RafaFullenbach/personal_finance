using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;

namespace personal_finance.Application.UseCases.Categories.UpdateCategory
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
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.CategoryInvalidId);

            if (string.IsNullOrWhiteSpace(command.Name))
                throw ValidationException.Invalid("Nome é obrigatório.", ErrorCodes.CategoryInvalidName);

            if (!Enum.TryParse<CategoryType>(command.Type, true, out var newType))
                throw ValidationException.Invalid($"Categoria do tipo '{command.Type}' é inválida.", ErrorCodes.CategoryInvalidType);

            var category = await _categories.GetByIdAsync(command.Id);
            if (category is null)
                throw NotFoundException.For("Categoria", command.Id);

            // ✅ Regra: só muda Type se não tiver uso
            if (category.Type != newType)
            {
                var used =
                    await _usage.HasTransactionsAsync(category.Id) ||
                    await _usage.HasBudgetsAsync(category.Id) ||
                    await _usage.HasRecurringTemplatesAsync(category.Id);

                if (used)
                    throw new BusinessRuleException("O tipo de categoria não pode ser alterado porque já está em uso.");
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

using personal_finance.Application.Errors;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using personal_finance.Application.Interfaces.Categories;

namespace personal_finance.Application.UseCases.Categories.CreateCategory
{
    public sealed class CreateCategoryHandler
    {
        private readonly ICategoryRepository _repo;

        public CreateCategoryHandler(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<CreateCategoryResult> HandleAsync(CreateCategoryCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw ValidationException.Invalid("Categoria é obrigatória.", ErrorCodes.CategoryInvalidName);

            if (!Enum.TryParse<CategoryType>(command.Type, true, out var type))
                throw ValidationException.Invalid($"Categoria do tipo '{command.Type}' é inválida.", ErrorCodes.CategoryInvalidType);

            var category = new Category(command.Name, type);
            await _repo.AddAsync(category);

            return new CreateCategoryResult
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type.ToString(),
                IsActive = category.IsActive
            };
        }
    }
}

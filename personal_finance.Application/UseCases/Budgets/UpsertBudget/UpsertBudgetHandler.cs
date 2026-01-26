using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Budgets;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Budgets.UpsertBudget
{
    public sealed class UpsertBudgetHandler
    {
        private readonly IBudgetRepository _budgets;
        private readonly ICategoryRepository _categories;

        public UpsertBudgetHandler(IBudgetRepository budgets, ICategoryRepository categories)
        {
            _budgets = budgets;
            _categories = categories;
        }

        public async Task<UpsertBudgetResult> HandleAsync(UpsertBudgetCommand command)
        {
            if (command.CategoryId == Guid.Empty)
                throw ValidationException.Invalid("Id da categoria é obrigatório.", ErrorCodes.BudgetInvalidCategory);

            if (command.Year < 2000 || command.Year > 2100)
                throw ValidationException.Invalid("Ano inválido.", ErrorCodes.BudgetInvalidPeriod);

            if (command.Month < 1 || command.Month > 12)
                throw ValidationException.Invalid("Mês inválido.", ErrorCodes.BudgetInvalidPeriod);

            if (command.LimitAmount <= 0)
                throw ValidationException.Invalid("O Valor limite deve ser maior que zero.", ErrorCodes.BudgetInvalidLimit);

            var category = await _categories.GetByIdAsync(command.CategoryId);
            if (category is null)
                throw NotFoundException.For("Categoria", command.CategoryId);

            if (!category.IsActive)
                throw new BusinessRuleException("Categoria desativada. Orçamentos não podem ser criados/alterados.");

            if (category.Type != CategoryType.Expense)
                throw ValidationException.Invalid("Os orçamentos são permitidos apenas para categorias de despesas.", ErrorCodes.BudgetInvalidCategoryType);

            var existing = await _budgets.GetByCategoryAndMonthAsync(command.CategoryId, command.Year, command.Month);

            if (existing is null)
            {
                var budget = new Budget(command.CategoryId, command.Year, command.Month, command.LimitAmount);
                await _budgets.AddAsync(budget);

                return new UpsertBudgetResult
                {
                    Id = budget.Id,
                    CategoryId = budget.CategoryId,
                    Year = budget.Year,
                    Month = budget.Month,
                    LimitAmount = budget.LimitAmount,
                    IsActive = budget.IsActive,
                    Action = "Created"
                };
            }
            else
            {
                existing.UpdateLimit(command.LimitAmount);
                await _budgets.UpdateAsync(existing);

                return new UpsertBudgetResult
                {
                    Id = existing.Id,
                    CategoryId = existing.CategoryId,
                    Year = existing.Year,
                    Month = existing.Month,
                    LimitAmount = existing.LimitAmount,
                    IsActive = existing.IsActive,
                    Action = "Updated"
                };
            }
        }
    }
}

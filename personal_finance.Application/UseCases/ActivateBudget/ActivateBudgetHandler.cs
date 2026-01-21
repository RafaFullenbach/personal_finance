using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.ActivateBudget
{
    public sealed class ActivateBudgetHandler
    {
        private readonly IBudgetRepository _budgets;

        public ActivateBudgetHandler(IBudgetRepository budgets)
        {
            _budgets = budgets;
        }

        public async Task<object> HandleAsync(ActivateBudgetCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id is required.", ErrorCodes.BudgetInvalidId);

            var budget = await _budgets.GetByIdAsync(command.Id);
            if (budget is null)
                throw NotFoundException.For("Budget", command.Id);

            budget.Activate();
            await _budgets.UpdateAsync(budget);

            return new { budget.Id, budget.IsActive };
        }
    }
}

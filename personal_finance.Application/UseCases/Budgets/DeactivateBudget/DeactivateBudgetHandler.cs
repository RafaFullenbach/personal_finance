using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.UseCases.Budgets.DeactivateBudget
{
    public sealed class DeactivateBudgetHandler
    {
        private readonly IBudgetRepository _budgets;

        public DeactivateBudgetHandler(IBudgetRepository budgets)
        {
            _budgets = budgets;
        }

        public async Task<object> HandleAsync(DeactivateBudgetCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.BudgetInvalidId);

            var budget = await _budgets.GetByIdAsync(command.Id);
            if (budget is null)
                throw NotFoundException.For("Orçamento", command.Id);

            budget.Deactivate();
            await _budgets.UpdateAsync(budget);

            return new { budget.Id, budget.IsActive };
        }
    }
}

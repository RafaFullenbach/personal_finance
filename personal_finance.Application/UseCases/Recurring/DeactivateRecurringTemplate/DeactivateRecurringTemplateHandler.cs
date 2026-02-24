using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Recurring;

namespace personal_finance.Application.UseCases.Recurring.DeactivateRecurringTemplate
{
    public sealed class DeactivateRecurringTemplateHandler
    {
        private readonly IRecurringTemplateRepository _repo;

        public DeactivateRecurringTemplateHandler(IRecurringTemplateRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeactivateRecurringTemplateResult> HandleAsync(DeactivateRecurringTemplateCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.RecurringInvalidId);

            var template = await _repo.GetByIdAsync(command.Id);
            if (template is null)
                throw NotFoundException.For("Modelo recorrente", command.Id);

            template.Deactivate();
            await _repo.UpdateAsync(template);

            return new DeactivateRecurringTemplateResult
            {
                Id = template.Id,
                IsActive = template.IsActive
            };
        }
    }
}

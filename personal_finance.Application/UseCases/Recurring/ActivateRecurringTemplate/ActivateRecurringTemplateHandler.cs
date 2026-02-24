using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Recurring;

namespace personal_finance.Application.UseCases.Recurring.ActivateRecurringTemplate
{
    public sealed class ActivateRecurringTemplateHandler
    {
        private readonly IRecurringTemplateRepository _repo;

        public ActivateRecurringTemplateHandler(IRecurringTemplateRepository repo)
        {
            _repo = repo;
        }

        public async Task<ActivateRecurringTemplateResult> HandleAsync(ActivateRecurringTemplateCommand command)
        {
            if (command.Id == Guid.Empty)
                throw ValidationException.Invalid("Id é obrigatório.", ErrorCodes.RecurringInvalidId);

            var template = await _repo.GetByIdAsync(command.Id);
            if (template is null)
                throw NotFoundException.For("Modelo recorrente", command.Id);

            template.Activate();
            await _repo.UpdateAsync(template);

            return new ActivateRecurringTemplateResult
            {
                Id = template.Id,
                IsActive = template.IsActive
            };
        }
    }
}

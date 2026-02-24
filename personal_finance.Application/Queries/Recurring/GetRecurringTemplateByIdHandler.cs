using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Recurring;

namespace personal_finance.Application.Queries.Recurring
{
    public sealed class GetRecurringTemplateByIdHandler
    {
        private readonly IRecurringTemplateQueryRepository _repo;

        public GetRecurringTemplateByIdHandler(IRecurringTemplateQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<RecurringTemplateListItemDto> HandleAsync(GetRecurringTemplateByIdQuery query)
        {
            var item = await _repo.GetByIdAsync(query.Id);

            if (item is null)
                throw NotFoundException.For("Modelo recorrente", query.Id);

            return item;
        }
    }
}

using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Recurring
{
    public sealed class GetAllRecurringTemplatesHandler
    {
        private readonly IRecurringTemplateQueryRepository _repo;
        public GetAllRecurringTemplatesHandler(IRecurringTemplateQueryRepository repo) => _repo = repo;

        public Task<IReadOnlyList<RecurringTemplateListItemDto>> HandleAsync(bool includeInactive = false)
            => _repo.GetAllAsync(includeInactive);
    }
}

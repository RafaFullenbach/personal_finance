using personal_finance.Application.Queries.Recurring;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.Recurring
{
    public interface IRecurringTemplateQueryRepository
    {
        Task<IReadOnlyList<RecurringTemplateListItemDto>> GetAllAsync(bool includeInactive = false);
    }
}

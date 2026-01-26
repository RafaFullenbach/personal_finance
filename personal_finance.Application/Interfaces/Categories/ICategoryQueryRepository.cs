using personal_finance.Application.Queries.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Interfaces.Categories
{
    public interface ICategoryQueryRepository
    {
        Task<IReadOnlyList<CategoryListItemDto>> GetAllAsync(bool includeInactive = false);

        Task<CategoryListItemDto?> GetByIdAsync(Guid id);
    }
}

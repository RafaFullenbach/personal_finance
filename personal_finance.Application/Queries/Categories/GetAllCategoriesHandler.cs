using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Categories
{
    public sealed class GetAllCategoriesHandler
    {
        private readonly ICategoryQueryRepository _repo;

        public GetAllCategoriesHandler(ICategoryQueryRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyList<CategoryListItemDto>> HandleAsync(bool includeInactive = true)
            => _repo.GetAllAsync(includeInactive);
    }
}

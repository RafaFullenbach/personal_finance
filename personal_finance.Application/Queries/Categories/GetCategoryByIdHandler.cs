using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace personal_finance.Application.Queries.Categories
{
    public sealed class GetCategoryByIdHandler
    {
        private readonly ICategoryQueryRepository _repo;

        public GetCategoryByIdHandler(ICategoryQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<CategoryListItemDto> HandleAsync(GetCategoryByIdQuery query)
        {
            var item = await _repo.GetByIdAsync(query.Id);
            if (item is null)
                throw NotFoundException.For("Category", query.Id);

            return item;
        }
    }
}

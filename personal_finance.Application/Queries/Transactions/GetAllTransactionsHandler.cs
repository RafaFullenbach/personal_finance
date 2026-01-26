using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.Queries.Common;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Domain.Enums;

namespace personal_finance.Application.Queries.Transactions
{
    public sealed class GetAllTransactionsHandler
    {
        private readonly ITransactionQueryRepository _repo;

        public GetAllTransactionsHandler(ITransactionQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<TransactionListItemDto>> HandleAsync(GetTransactionsQuery query)
        {
            if (query.Page <= 0)
                throw ValidationException.Invalid(
                    "Página precisa ser >= 1.",
                    ErrorCodes.QueryInvalidPagination);

            if (query.PageSize <= 0 || query.PageSize > 200)
                throw ValidationException.Invalid(
                    "Tamanho da página precisa ser entre 1 e 200.",
                    ErrorCodes.QueryInvalidPagination);

            if (!string.IsNullOrWhiteSpace(query.Type) &&
                !Enum.TryParse<TransactionType>(query.Type, ignoreCase: true, out _))
            {
                throw ValidationException.Invalid(
                    $"Tipo '{query.Type}' é inválido.",
                    ErrorCodes.TransactionInvalidType);
            }

            if (!string.IsNullOrWhiteSpace(query.Status) &&
                !Enum.TryParse<TransactionStatus>(query.Status, ignoreCase: true, out _))
            {
                throw ValidationException.Invalid(
                    $"Status '{query.Status}' é inválido.",
                    ErrorCodes.TransactionInvalidStatus);
            }

            var sortBy = (query.SortBy ?? "transactionDate").Trim().ToLowerInvariant();
            var allowedSort = new HashSet<string> { "transactiondate", "amount" };
            if (!allowedSort.Contains(sortBy))
            {
                throw ValidationException.Invalid(
                    $"Organizar por '{query.SortBy}' é inválido.",
                    ErrorCodes.QueryInvalidSort);
            }

            var order = (query.Order ?? "desc").Trim().ToLowerInvariant();
            if (order is not ("asc" or "desc"))
            {
                throw ValidationException.Invalid(
                    $"Ordenação por '{query.Order}' é inválido.",
                    ErrorCodes.QueryInvalidSort);
            }

            return await _repo.GetAsync(query);
        }
    }
}

using personal_finance.Application.Errors;
using personal_finance.Application.Exceptions;
using personal_finance.Application.Interfaces;
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
            // Pagination guards
            if (query.Page <= 0)
                throw ValidationException.Invalid(
                    "Page must be >= 1.",
                    ErrorCodes.QueryInvalidPagination);

            if (query.PageSize <= 0 || query.PageSize > 200)
                throw ValidationException.Invalid(
                    "PageSize must be between 1 and 200.",
                    ErrorCodes.QueryInvalidPagination);

            // Type guard (aceita null)
            if (!string.IsNullOrWhiteSpace(query.Type) &&
                !Enum.TryParse<TransactionType>(query.Type, ignoreCase: true, out _))
            {
                throw ValidationException.Invalid(
                    $"Type '{query.Type}' is invalid.",
                    ErrorCodes.TransactionInvalidType);
            }

            // Status guard (aceita null)
            if (!string.IsNullOrWhiteSpace(query.Status) &&
                !Enum.TryParse<TransactionStatus>(query.Status, ignoreCase: true, out _))
            {
                throw ValidationException.Invalid(
                    $"Status '{query.Status}' is invalid.",
                    ErrorCodes.TransactionInvalidStatus);
            }

            // Sort whitelist
            var sortBy = (query.SortBy ?? "transactionDate").Trim().ToLowerInvariant();
            var allowedSort = new HashSet<string> { "transactiondate", "amount" };
            if (!allowedSort.Contains(sortBy))
            {
                throw ValidationException.Invalid(
                    $"SortBy '{query.SortBy}' is invalid.",
                    ErrorCodes.QueryInvalidSort);
            }

            // Order whitelist
            var order = (query.Order ?? "desc").Trim().ToLowerInvariant();
            if (order is not ("asc" or "desc"))
            {
                throw ValidationException.Invalid(
                    $"Order '{query.Order}' is invalid.",
                    ErrorCodes.QueryInvalidSort);
            }

            // ✅ chama o repo
            return await _repo.GetAsync(query);
        }
    }
}

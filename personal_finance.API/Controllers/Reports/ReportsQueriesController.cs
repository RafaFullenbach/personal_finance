using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.Queries.Reports.Accounts;
using personal_finance.Application.Queries.Reports.Balance;
using personal_finance.Application.Queries.Reports.Budgets;
using personal_finance.Application.Queries.Reports.CategorySummary;
using personal_finance.Application.Queries.Reports.MonthlySummary;

namespace personal_finance.API.Controllers.Reports
{
    [ApiController]
    [Route("reports")]
    [ApiExplorerSettings(GroupName = "Queries")]
    public class ReportsQueriesController : ControllerBase
    {
        private readonly GetMonthlySummaryHandler _handler;
        private readonly GetBalanceHandler _balanceHandler;
        private readonly GetAccountBalanceHandler _accountBalanceHandler;

        public ReportsQueriesController(GetMonthlySummaryHandler handler, GetBalanceHandler balanceHandler,
            GetAccountBalanceHandler accountBalanceHandler)
        {
            _handler = handler;
            _balanceHandler = balanceHandler;
            _accountBalanceHandler = accountBalanceHandler;
        }

        [HttpGet("monthly-summary")]
        public async Task<IActionResult> GetMonthlySummary(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var result = await _handler.HandleAsync(new GetMonthlySummaryQuery
            {
                Year = year,
                Month = month
            });

            return Ok(result);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance([FromQuery] DateTime date)
        {
            var result = await _balanceHandler.HandleAsync(new GetBalanceQuery
            {
                Date = date
            });

            return Ok(result);
        }

        [HttpGet("account-balance")]
        public async Task<IActionResult> GetAccountBalance(
            [FromQuery] Guid accountId,
            [FromQuery] DateTime date)
        {
            var result = await _accountBalanceHandler.HandleAsync(new GetAccountBalanceQuery
            {
                AccountId = accountId,
                Date = date
            });

            return Ok(result);
        }

        [HttpGet("category-summary")]
        public async Task<IActionResult> GetCategorySummary(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromQuery] string? type,
            [FromServices] GetCategorySummaryHandler handler)
        {
            var result = await handler.HandleAsync(new GetCategorySummaryQuery
            {
                Year = year,
                Month = month,
                Type = type
            });

            return Ok(result);
        }

        [HttpGet("budget-vs-actual")]
        public async Task<IActionResult> GetBudgetVsActual(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromServices] GetBudgetVsActualHandler handler)
        {
            var result = await handler.HandleAsync(new GetBudgetVsActualQuery
            {
                Year = year,
                Month = month
            });

            return Ok(result);
        }
    }
}

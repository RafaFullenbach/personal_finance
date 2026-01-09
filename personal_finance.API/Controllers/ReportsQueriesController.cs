using Microsoft.AspNetCore.Mvc;
using personal_finance.Application.Queries.Reports;

namespace personal_finance.API.Controllers
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
    }
}

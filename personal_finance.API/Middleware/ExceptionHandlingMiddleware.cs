using personal_finance.Domain.Exceptions;
using personal_finance.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace personal_finance.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessRuleException ex)
            {
                await WriteProblem(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await WriteProblem(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception)
            {
                await WriteProblem(context, HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static async Task WriteProblem(HttpContext context, HttpStatusCode status, string message)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)status;

            var payload = new
            {
                type = "about:blank",
                title = status.ToString(),
                status = (int)status,
                detail = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}

using Standard.API.PSQL.Domain.Exceptions;
using System.Net;
using System.Text.Json.Nodes;

namespace Standard.API.PSQL.Application.Middlewares
{

    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);

            var jsonObject = new JsonObject(new[] { KeyValuePair.Create<string, JsonNode?>("message", exception.InnerException?.Message ?? exception.Message), });

            await context.Response.WriteAsync(jsonObject.ToString());
        }

        public HttpStatusCode GetStatusCode(Exception exception) => exception is not BaseException internalException ? HttpStatusCode.InternalServerError : internalException.StatusCode;
    }
}
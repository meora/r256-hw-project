using Grpc.Core;
using System.Net;
using System.Text.Json;

namespace Ozon.Route256.Practice.GatewayService.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (RpcException ex)
            {
                switch (ex.StatusCode)
                {
                    case StatusCode.NotFound:
                        await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound);
                        break;
                    case StatusCode.FailedPrecondition:
                        await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
                        break;
                    default:
                        await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
                        break;
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(exception.Message));
        }
    }
}

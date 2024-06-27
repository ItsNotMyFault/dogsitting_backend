using System.Net;
using System.Text.Json;

namespace dogsitting_backend.Startup.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                var message = error?.Message;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(new { message, code = response.StatusCode });
                await response.WriteAsync(result);
            }
        }
    }
}

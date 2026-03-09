using PruebaComerciantes.Application.DTOs;
using System.Net;
using System.Text.Json;

namespace PruebaComerciantes.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;

                _logger.LogError(ex, "Error no controlado");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = message,
                    Data = ex.StackTrace
                };

                var json = JsonSerializer.Serialize(response);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(json);
            }
        }
    }
}

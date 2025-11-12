using GerenciamentoProjeto.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace GerenciamentoProjeto.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _env = env;
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
                _logger.LogError(ex, "Erro inesperado");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            object response;

            var dev = _env.IsDevelopment();

            switch (exception)
            {
                case ValidationException ve:
                    status = HttpStatusCode.BadRequest;
                    response = new { message = ve.Message, errors = ve.Erros };
                    break;

                case NotFoundException nf:
                    status = HttpStatusCode.NotFound;
                    response = new { message = nf.Message };
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    response = new { message = "Ocorreu um erro interno no servidor." };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}

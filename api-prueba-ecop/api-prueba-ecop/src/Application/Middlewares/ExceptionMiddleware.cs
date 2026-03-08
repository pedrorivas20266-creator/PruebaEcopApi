using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Models.Responses;
using System.Net;
using System.Text.Json;

namespace api_prueba_ecop.src.Application.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        int errorCode;
        string message;

        switch (exception)
        {
            case DomainException ex:
                status = HttpStatusCode.BadRequest;
                errorCode = 400;
                message = ex.Message;
                break;

            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                errorCode = 401;
                message = "No autorizado.";
                break;

            case ArgumentException:
                status = HttpStatusCode.BadRequest;
                errorCode = 400;
                message = exception.Message;
                break;

            case KeyNotFoundException:
                status = HttpStatusCode.NotFound;
                errorCode = 404;
                message = "Recurso no encontrado.";
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                errorCode = 500;
                message = "Ocurrió un error interno en el servidor.";
                break;
        }

        var response = new ErrorResponse
        {
            ErrorCode = errorCode,
            ErrorDescription = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}
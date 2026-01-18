using SecureFlow.Application.Common.Exceptions;
using SecureFlow.Shared.Models;
using System.Net;
using System.Text.Json;

namespace SecureFlow.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
      HttpContext context,
      Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            ForbiddenException => HttpStatusCode.Forbidden,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        IEnumerable<string>? errors = null;
        
        if (exception is ValidationException ve)
        {
            // Flatten ValidationException errors dictionary to string array
            errors = ve.Errors
                .SelectMany(kvp => kvp.Value.Select(v => $"{kvp.Key}: {v}"))
                .ToArray();
        }

        var response = new ApiResponse<object>(
            message: exception.Message,
            errors: errors ?? (exception is ValidationException ? null : new[] { exception.Message }));

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, options));
    }
}

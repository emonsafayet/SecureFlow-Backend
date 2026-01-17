using System.Net;
using System.Text.Json;
using SecureFlow.Application.Common.Exceptions;

namespace SecureFlow.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
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
            _logger.LogError(ex, "Unhandled exception");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            ForbiddenException => HttpStatusCode.Forbidden,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        var problem = new
        {
            type = "https://httpstatuses.com/" + (int)statusCode,
            title = statusCode.ToString(),
            status = (int)statusCode,
            detail = exception.Message
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(problem));
    }
}
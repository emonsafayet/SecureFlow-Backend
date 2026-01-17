using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecureFlow.Application.Common.Logging;

namespace SecureFlow.Infrastructure.Logging;

internal class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;

    public AppLogger(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
        => _logger.LogInformation(message);

    public void LogWarning(string message)
        => _logger.LogWarning(message);

    public void LogError(Exception ex, string message)
        => _logger.LogError(ex, message);
}

public static class LoggingExtensions
{
    public static IServiceCollection AddAppLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        return services;
    }
}
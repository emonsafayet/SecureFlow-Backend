using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureFlow.Application.Common.Authorization;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Infrastructure.Caching;
using SecureFlow.Infrastructure.Email;
using SecureFlow.Infrastructure.Persistence;
using SecureFlow.Infrastructure.Security;
using StackExchange.Redis;
 

namespace SecureFlow.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // -----------------------------
        // DbContext
        // -----------------------------
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppDbContext>(sp =>
            sp.GetRequiredService<AppDbContext>());

        // -----------------------------
        // Generic Repository
        // -----------------------------
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 

        
        // -----------------------------
        // Security & Auth
        // -----------------------------
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();  


        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // -----------------------------
        // Email
        // -----------------------------
        services.Configure<SmtpSettings>(options => configuration.GetSection("Smtp").Bind(options));
        services.AddScoped<IEmailService, SmtpEmailService>();


        // -----------------------------
        // Redis
        // -----------------------------
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = ConfigurationOptions.Parse(
                configuration.GetConnectionString("Redis")!,
                true);

            options.Ssl = true;
            options.AbortOnConnectFail = false;
            options.ConnectRetry = 5;
            options.ReconnectRetryPolicy = new ExponentialRetry(5000);

            return ConnectionMultiplexer.Connect(options);
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
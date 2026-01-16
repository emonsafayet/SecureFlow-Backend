using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection; 
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Infrastructure.Email;
using SecureFlow.Infrastructure.Persistence;
using SecureFlow.Infrastructure.Security; 

namespace SecureFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        // Generic Repository registration
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Security & Auth
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        
      
        // Email 
        services.Configure<SmtpSettings>(options => configuration.GetSection("Smtp").Bind(options));
        services.AddScoped<IEmailService, SmtpEmailService>();
         


        return services;
    } 
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;
using SecureFlow.Application.Auth.Login;
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
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

      
        services.Configure<SmtpSettings>(options => configuration.GetSection("Smtp").Bind(options));

        //Services       
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

       

        services.AddScoped<IEmailService, SmtpEmailService>();
        return services;
    }
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LoginService>();

        return services;
    }
}

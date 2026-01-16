using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SecureFlow.Application;

public static class DependencyInjection
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR (CQRS)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);


        return services;
    }
}

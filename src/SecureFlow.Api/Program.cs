using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SecureFlow.Application;
using SecureFlow.Application.Common.Authorization;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Infrastructure;
using SecureFlow.Infrastructure.Persistence;
using SecureFlow.Shared.Authorization;
using Microsoft.AspNetCore.Authorization; 
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Swagger (Swashbuckle ONLY)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // DI
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
         
        
        builder.Services.AddAuthorization(options =>
        {
            foreach (var permission in Permissions.All)
            {
                options.AddPolicy(permission.Name, policy =>
                {
                    policy.Requirements.Add(
                        new PermissionRequirement(permission.Name));
                });
            }
        });
        builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        
        // JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                ),

                ClockSkew = TimeSpan.Zero
            };
        });
        var app = builder.Build();

        // DB seed (dev only – later we’ll guard this)
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();
            await AppDbContextSeed.SeedAsync(context, hasher);
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SecureFlow API v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
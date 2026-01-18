using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecureFlow.Application;
using SecureFlow.Application.Common.Authorization;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Infrastructure;
using SecureFlow.Infrastructure.Persistence;
using SecureFlow.Shared.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Serilog;
using SecureFlow.Api.Middleware;
public class Program
{
    public static async Task Main(string[] args)
    {
        // Logging file created here to capture startup logs
        Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File(
                            "Logs/secureflow-.txt",
                            rollingInterval: RollingInterval.Day)
                        .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        // --------------------------------------------------
        // Attach Serilog to ASP .NET pipeline
        // --------------------------------------------------
        builder.Host.UseSerilog();


        // --------------------------------------------------
        // Controller
        // --------------------------------------------------
        builder.Services.AddControllers();

        // --------------------------------------------------
        // Swagger (ONCE, WITH JWT SUPPORT)
        // --------------------------------------------------
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SecureFlow API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // --------------------------------------------------
        // Application & Infrastructure DI
        // --------------------------------------------------
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        // --------------------------------------------------
        // JWT Authentication (MUST COME BEFORE AUTHORIZATION)
        // --------------------------------------------------
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

                    // IMPORTANT for CurrentUserService
                    NameClaimType = JwtRegisteredClaimNames.Sub,
                    ClockSkew = TimeSpan.Zero
                };
            });

        // --------------------------------------------------
        // Permission-based Authorization Policies
        // --------------------------------------------------
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


        // --------------------------------------------------
        // Build app
        // --------------------------------------------------


        var app = builder.Build();

        // --------------------------------------------------
        // Database Seed (DEV ONLY)
        // --------------------------------------------------
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();
            await AppDbContextSeed.SeedAsync(context, hasher);
        }

        // --------------------------------------------------
        // Middleware pipeline
        // --------------------------------------------------
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
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();   // MUST be first
        app.UseAuthorization();    // MUST be after authentication

        app.MapControllers();

        app.Run();
    }
}

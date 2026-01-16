using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Entities;

namespace SecureFlow.Infrastructure.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(
        AppDbContext context,
        IPasswordHasherService passwordHasher)
    {
        if (context.Users.Any())
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@secureflow.com",
            PasswordHash = passwordHasher.Hash("Admin@123"),
            IsActive = true
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}

using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Auth;


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
            UserId = 1,
            Email = "admin@secureflow.com",
            FirstName= "First_Name",
            LastName= "Last_Name",
            PasswordHash = passwordHasher.Hash("Admin@123"),
            IsActive = true
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}

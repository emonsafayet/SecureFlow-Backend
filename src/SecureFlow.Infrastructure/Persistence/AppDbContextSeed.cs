using Microsoft.EntityFrameworkCore;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Auth;
using SecureFlow.Shared.Authorization;

namespace SecureFlow.Infrastructure.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(
        AppDbContext context,
        IPasswordHasherService passwordHasher)
    {
        // 1️ Permissions
        await SeedPermissionsAsync(context);

        // 2️ Roles
        await SeedRolesAsync(context);

        // 3️ Admin user
        await SeedAdminUserAsync(context, passwordHasher);

        // 4️ Assign ALL permissions to Admin role
        await SeedAdminRolePermissionsAsync(context);
    }

    // -------------------------------
    // PERMISSIONS
    // -------------------------------
    private static async Task SeedPermissionsAsync(AppDbContext db)
    {
        if (await db.Permissions.AnyAsync())
            return;

        var permissions = Permissions.All.Select(p =>
            new PermissionEntity
            {
                Name = p.Name,          // Permissions.Users.View
                Action = p.Action,      // View
                Resource = p.Resource,  // Users
                Description = p.Description
            }).ToList();

        db.Permissions.AddRange(permissions);
        await db.SaveChangesAsync();
    }

    // -------------------------------
    // ROLES
    // -------------------------------
    private static async Task SeedRolesAsync(AppDbContext db)
    {
        if (await db.Roles.AnyAsync())
            return;

        db.Roles.Add(new Role
        {
            Name = "Admin"
        });

        await db.SaveChangesAsync();
    }

    // -------------------------------
    // ADMIN USER
    // -------------------------------
    private static async Task SeedAdminUserAsync(
        AppDbContext db,
        IPasswordHasherService passwordHasher)
    {
        if (await db.Users.AnyAsync())
            return;

        var admin = new User
        {
            Email = "admin@secureflow.com",
            FirstName = "First",
            LastName = "Admin",
            PasswordHash = passwordHasher.Hash("Admin@123"),
            IsActive = true
        };

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }

    // -------------------------------
    // ADMIN ROLE → ALL PERMISSIONS
    // -------------------------------
    private static async Task SeedAdminRolePermissionsAsync(AppDbContext db)
    {
        var adminRole = await db.Roles
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        if (adminRole == null)
            return;

        // Prevent duplicate assignment
        if (await db.RolePermissions.AnyAsync(rp => rp.RoleId == adminRole.Id))
            return;

        var permissions = await db.Permissions.ToListAsync();

        foreach (var permission in permissions)
        {
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = permission.Id
            });
        }

        await db.SaveChangesAsync();
    }
}

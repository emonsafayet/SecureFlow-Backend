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

        // 2 ️ Seed Menus
        await SeedMenusAsync(context);

        // 3 Seed Menu Permissions
        await SeedMenuPermissionsAsync(context);

        // 4 Roles
        await SeedRolesAsync(context);

        // 5 Admin user
        await SeedAdminUserAsync(context, passwordHasher);

        // 6 Assign Admin role to Admin user
        await SeedAdminUserRoleAsync(context);

        // 7 Assign ALL permissions to Admin role
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
    // ADMIN USER → ADMIN ROLE
    // -------------------------------
    private static async Task SeedAdminUserRoleAsync(AppDbContext db)
    {
        var adminUser = await db.Users.FirstOrDefaultAsync();
        var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

        if (adminUser == null || adminRole == null)
            return;

        if (await db.UserRoles.AnyAsync(ur =>
            ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id))
            return;

        db.UserRoles.Add(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });

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

    // -------------------------------
    // SEED MENUS
    // -------------------------------
    private static async Task SeedMenusAsync(AppDbContext db)
    {
        if (await db.Menus.AnyAsync())
            return;

        var menus = new List<Menu>
    {
        new Menu
        {
            Name = "Dashboard",
            Url = "/dashboard",
            Order = 1,
            IsActive =true
        },
        new Menu
        {
            Name = "Users",
            Url = "/users",
            Order = 2,
            IsActive =true
        }
    };

        db.Menus.AddRange(menus);
        await db.SaveChangesAsync();
    }


    // -------------------------------
    // MENUS PERMISSIONS
    // -------------------------------
    private static async Task SeedMenuPermissionsAsync(AppDbContext db)
    {
        if (db.MenuPermissions.Any())
            return;

        var dashboard = await db.Menus.FirstAsync(m => m.Name == "Dashboard");
        var users = await db.Menus.FirstAsync(m => m.Name == "Users"); 

        var permissions = await db.Permissions.ToListAsync();

        var viewUsers = permissions.First(p =>
            p.Name == Permissions.ViewUsers.Name); 

        var mappings = new List<MenuPermission>
    {
        new MenuPermission
        {
            MenuId = dashboard.Id,
            PermissionId = viewUsers.Id
        },
        new MenuPermission
        {
            MenuId = users.Id,
            PermissionId = viewUsers.Id
        }
    };

        db.MenuPermissions.AddRange(mappings);
        await db.SaveChangesAsync();
    }

}

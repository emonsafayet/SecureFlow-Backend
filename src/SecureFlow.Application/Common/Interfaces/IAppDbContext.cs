using Microsoft.EntityFrameworkCore;
using SecureFlow.Domain.Auth;

namespace SecureFlow.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Menu> Menus { get; } // Ensure 'Menu' refers to the correct type from the namespace
    DbSet<MenuPermission> MenuPermissions { get; }
    DbSet<Role> Roles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<PermissionEntity> Permissions { get; } 
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Microsoft.EntityFrameworkCore;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Auth;
using SecureFlow.Domain.Common.Abstractions;
using SecureFlow.Domain.Common.Markers;
using System.Linq.Expressions;

namespace SecureFlow.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly ICurrentUserService _currentUser;
    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<PermissionEntity> Permissions => Set<PermissionEntity>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuPermission> MenuPermissions => Set<MenuPermission>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1️.Schema mapping via marker interfaces
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clr = entityType.ClrType;

            if (typeof(IAuthEntity).IsAssignableFrom(clr))
                modelBuilder.Entity(clr).ToTable(entityType.GetTableName(), "auth");

            if (typeof(IBusinessEntity).IsAssignableFrom(clr))
                modelBuilder.Entity(clr).ToTable(entityType.GetTableName(), "business");

            //if (typeof(ILookupEntity).IsAssignableFrom(clr))
            //    modelBuilder.Entity(clr).ToTable(entityType.GetTableName(), "lu");

            //if (typeof(IAuditEntity).IsAssignableFrom(clr))
            //    modelBuilder.Entity(clr).ToTable(entityType.GetTableName(), "audit");
        }

        // 2️. Global Soft Delete Filter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(CreateIsDeletedFilter(entityType.ClrType));
            }
        }

        modelBuilder.Entity<User>(entity =>
        {
            // 1️ Primary Key (DB identity)
            entity.HasKey(u => u.Id);

            // 2️ Public ID (GUID) – generated in code, NOT DB
            entity.Property(u => u.UserId)
                  .IsRequired();

            entity.HasIndex(u => u.UserId)
                  .IsUnique();

            // 3️ Email should be unique
            entity.HasIndex(u => u.Email)
                  .IsUnique();
            // 4️ Optional profile image
            entity.HasOne(u => u.ProImgEvidence)
                  .WithMany()
                  .HasForeignKey(u => u.ProImgEvidenceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(x => new { x.RoleId, x.PermissionId });

            entity.HasOne(x => x.Role)
                  .WithMany(r => r.RolePermissions)
                  .HasForeignKey(x => x.RoleId);

            entity.HasOne(x => x.Permission)
                  .WithMany()
                  .HasForeignKey(x => x.PermissionId);
        });

        modelBuilder.Entity<MenuPermission>()
                    .HasKey(x => new { x.MenuId, x.PermissionId });

    }
    private static LambdaExpression CreateIsDeletedFilter(Type type)
    {
        var param = Expression.Parameter(type, "e");

        var deletedOnProp = Expression.Property(
            param,
            typeof(ISoftDelete).GetProperty(nameof(ISoftDelete.DeletedOn))!
        );

        var body = Expression.Equal(
            deletedOnProp,
            Expression.Constant(null)
        );

        return Expression.Lambda(body, param);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var userId = _currentUser?.UserId ?? 0;

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = now;
                entry.Entity.CreatedBy = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedOn = now;
                entry.Entity.LastModifiedBy = userId;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }


}


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
        : base(options) {
        _currentUser = currentUser;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();


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
            entity.HasKey(u => u.UserId);

            entity.Property(u => u.UserId)
                  .ValueGeneratedOnAdd();

            entity.HasIndex(u => u.Id)
                  .IsUnique();
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


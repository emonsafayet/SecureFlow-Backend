using Microsoft.EntityFrameworkCore;
using SecureFlow.Domain.Auth;

namespace SecureFlow.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

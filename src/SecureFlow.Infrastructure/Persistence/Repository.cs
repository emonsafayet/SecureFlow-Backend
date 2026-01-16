using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Abstractions;
using SecureFlow.Infrastructure.Persistence;
using System.Linq.Expressions;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _db;
    protected readonly DbSet<T> _set;

    public Repository(AppDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public IQueryable<T> Query(bool ignoreGlobalFilters = false)
        => ignoreGlobalFilters ? _set.IgnoreQueryFilters() : _set;

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _set.FindAsync(new object[] { id }, ct);

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(predicate, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _set.AddAsync(entity, ct);

    public void Update(T entity)
        => _set.Update(entity);

    public void SoftDelete(T entity)
    {
        if (entity is ISoftDelete softDelete)
        {
            softDelete.DeletedOn = DateTime.UtcNow;
        }
        else
        {
            _set.Remove(entity);
        }
    }

    public void Restore(T entity)
    {
        if (entity is ISoftDelete softDelete)
        {
            softDelete.DeletedOn = null;
            softDelete.DeletedBy = null;
            _set.Update(entity);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken ct = default)
        => await _db.Database.BeginTransactionAsync(ct);
}

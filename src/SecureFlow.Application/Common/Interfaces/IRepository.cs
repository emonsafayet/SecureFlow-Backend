using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using SecureFlow.Domain.Common.Base;

namespace SecureFlow.Application.Common.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> Query(bool ignoreGlobalFilters = false);

    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);

    void SoftDelete(T entity);
    void Restore(T entity);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}
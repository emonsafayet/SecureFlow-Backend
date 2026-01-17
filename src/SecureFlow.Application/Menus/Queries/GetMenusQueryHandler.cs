using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Application.Common.Caching;
using SecureFlow.Application.Common.Models;

namespace SecureFlow.Application.Menus.Queries.GetMenus;

public class GetMenusQueryHandler
    : IRequestHandler<GetMenusQuery, Result<List<MenuDto>>>
{
    private readonly IAppDbContext _db;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;
    private readonly CacheOptions _cacheOptions;

    public GetMenusQueryHandler(
        IAppDbContext db,
        ICurrentUserService currentUser,
        ICacheService cache,
        IOptions<CacheOptions> cacheOptions)
    {
        _db = db;
        _currentUser = currentUser;
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<Result<List<MenuDto>>> Handle(
        GetMenusQuery request,
        CancellationToken cancellationToken)
    {
        // 0️⃣ Guard: user must have permissions
        var userPermissions = _currentUser.Permissions
            .OrderBy(p => p)
            .ToArray();

        if (userPermissions.Length == 0)
        {
            return Result<List<MenuDto>>.Failure(
                "User has no permissions assigned");
        }

        // 1️ Build cache key
        var permissionHash = string.Join("|", userPermissions);
        var cacheKey = CacheKeys.MenusByPermissions(permissionHash);

        // 2️ Redis first
        var cached = await _cache.GetAsync<List<MenuDto>>(cacheKey);
        if (cached != null)
        {
            return Result<List<MenuDto>>.Success(cached);
        }

        // 3️ DB
        var menus = await _db.Menus
            .AsNoTracking()
            .Where(m =>
                m.MenuPermissions.Any(mp =>
                    userPermissions.Contains(mp.Permission.Name)))
            .OrderBy(m => m.Order)
            .Select(m => new MenuDto
            {
                Id = m.Id,
                Name = m.Name,
                Url = m.Url,
                Order = m.Order
            })
            .ToListAsync(cancellationToken);

        // 4️ Cache result
        await _cache.SetAsync(
            cacheKey,
            menus,
            TimeSpan.FromMinutes(_cacheOptions.DefaultExpirationMinutes));

        return Result<List<MenuDto>>.Success(menus);
    }
}
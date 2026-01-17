using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Application.Common.Caching;

namespace SecureFlow.Application.Menus.Queries.GetMenus;

public class GetMenusQueryHandler
    : IRequestHandler<GetMenusQuery, List<MenuDto>>
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

    public async Task<List<MenuDto>> Handle(
        GetMenusQuery request,
        CancellationToken cancellationToken)
    { 

        var permissions = _currentUser.Permissions
            .OrderBy(p => p)
            .ToArray();

        var permissionHash = string.Join("|", permissions);
        var cacheKey = CacheKeys.MenusByPermissions(permissionHash);


        // 1️ Redis first
        var cached = await _cache.GetAsync<List<MenuDto>>(cacheKey);
        if (cached != null)
            return cached;

        // 2️ DB
        var userPermissions = _currentUser.Permissions;

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

        // 3️ Cache with configurable TTL
        await _cache.SetAsync(
            cacheKey,
            menus,
            TimeSpan.FromMinutes(_cacheOptions.DefaultExpirationMinutes));

        return menus;
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecureFlow.Application.Common.Caching;
using SecureFlow.Application.Common.Exceptions;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Application.Menus.DTOs;
using SecureFlow.Shared.Models;

namespace SecureFlow.Application.Menus.Queries.GetMenus;

public class GetMenusQueryHandler
    : IRequestHandler<GetMenusQuery, PaginationResponse<MenuDto>>
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

    public async Task<PaginationResponse<MenuDto>> Handle(
        GetMenusQuery request,
        CancellationToken cancellationToken)
    {
        // 0️⃣ Guard: user must have permissions
        var userPermissions = _currentUser.Permissions
            .OrderBy(p => p)
            .ToArray();

        if (userPermissions.Length == 0)
        {
            throw new ForbiddenException("User has no permissions assigned");
        }

        // 1️ Build cache key
        var permissionHash = string.Join("|", userPermissions);
        var cacheKey = CacheKeys.MenusByPermissions(permissionHash);

        // 2️ Get all menus (from cache or DB)
        List<MenuDto> allMenus;
        var cached = await _cache.GetAsync<List<MenuDto>>(cacheKey);
        if (cached != null)
        {
            allMenus = cached;
        }
        else
        {
            // 3️ DB query
            allMenus = await _db.Menus
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
                allMenus,
                TimeSpan.FromMinutes(_cacheOptions.DefaultExpirationMinutes));
        }

        // 5️ Apply pagination
        var filter = request.Filter;
        var totalCount = allMenus.Count;

        if (!filter.HasPaging)
        {
            // Return all if no pagination requested
            return new PaginationResponse<MenuDto>(
                allMenus,
                totalCount,
                pageNumber: 1,
                pageSize: totalCount);
        }

        var pageNumber = filter.PageNumber;
        var pageSize = filter.PageSize;
        var skip = (pageNumber - 1) * pageSize;

        var pagedMenus = allMenus
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return new PaginationResponse<MenuDto>(
            pagedMenus,
            totalCount,
            pageNumber,
            pageSize);
    }
}
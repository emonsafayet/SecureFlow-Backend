using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Auth;
using System;

namespace SecureFlow.Application.Menus.Queries;

public class GetMenusQueryHandler
    : IRequestHandler<GetMenusQuery, List<MenuDto>>
{
    private readonly IAppDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetMenusQueryHandler(
        IAppDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<MenuDto>> Handle(
        GetMenusQuery request,
        CancellationToken cancellationToken)
    {
        var userPermissions = _currentUser.Permissions;

        if (!userPermissions.Any())
            return new List<MenuDto>();

        var menus = await _db.Menus
            .AsNoTracking()
            .Where(menu =>
                menu.MenuPermissions.Any(mp =>
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

        return menus;
    }
}
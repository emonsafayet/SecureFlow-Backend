using MediatR;
using SecureFlow.Application.Common.Models;

namespace SecureFlow.Application.Menus.Queries.GetMenus;

public record GetMenusQuery
    : IRequest<Result<List<MenuDto>>>;
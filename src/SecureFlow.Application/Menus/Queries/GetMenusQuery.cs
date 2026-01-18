using MediatR;
using SecureFlow.Application.Menus.DTOs;
using SecureFlow.Shared.Models;

namespace SecureFlow.Application.Menus.Queries.GetMenus;

public record GetMenusQuery(PaginationFilter Filter)
    : IRequest<PaginationResponse<MenuDto>>;
using MediatR;
public record GetMenusQuery : IRequest<List<MenuDto>>;
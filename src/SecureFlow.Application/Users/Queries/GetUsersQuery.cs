using MediatR;
using SecureFlow.Application.Users;
using SecureFlow.Shared.Models;

namespace SecureFlow.Application.Auth.Users.Queries;

public record GetUsersQuery(PaginationFilter Filter)
    : IRequest<PaginationResponse<UserDto>>;

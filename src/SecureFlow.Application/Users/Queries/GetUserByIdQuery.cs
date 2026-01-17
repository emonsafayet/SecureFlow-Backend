using MediatR;
using SecureFlow.Application.Common.Models;
using SecureFlow.Application.Users;

namespace SecureFlow.Application.Auth.Users.Queries;

public record GetUserByIdQuery(int UserId)
    : IRequest<Result<UserDto>>;

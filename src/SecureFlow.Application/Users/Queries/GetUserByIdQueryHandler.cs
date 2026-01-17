using AutoMapper;
using MediatR;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Application.Common.Models;
using SecureFlow.Application.Users;
using SecureFlow.Domain.Auth;

namespace SecureFlow.Application.Auth.Users.Queries;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IRepository<User> _users;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(
        IRepository<User> users,
        IMapper mapper)
    {
        _users = users;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(request.UserId, ct);

        if (user == null)
        {
            return Result<UserDto>.Failure(
                $"User with ID {request.UserId} was not found");
        }

        var dto = _mapper.Map<UserDto>(user);

        return Result<UserDto>.Success(dto);
    }
}
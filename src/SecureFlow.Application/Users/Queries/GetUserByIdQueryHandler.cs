using AutoMapper;
using MediatR;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Application.Users;
using SecureFlow.Domain.Auth;

namespace SecureFlow.Application.Auth.Users.Queries;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserDto>
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

    public async Task<UserDto> Handle(
        GetUserByIdQuery request,
        CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(request.UserId, ct)
            ?? throw new KeyNotFoundException("User not found");

        return _mapper.Map<UserDto>(user);
    }
}

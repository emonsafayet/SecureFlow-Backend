using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureFlow.Application.Auth.DTOs;
using SecureFlow.Application.Common.Interfaces;

namespace SecureFlow.Application.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IAppDbContext _db;
    private readonly IPasswordHasherService _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public LoginCommandHandler(
        IAppDbContext db,
        IPasswordHasherService hasher,
        IJwtTokenGenerator jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<LoginResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user == null || !_hasher.Verify(user.PasswordHash, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return new LoginResponseDto
        {
            AccessToken = _jwt.GenerateToken(user),
            RefreshToken = Guid.NewGuid().ToString()
        };
    }
}

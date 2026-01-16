using Microsoft.EntityFrameworkCore;
using SecureFlow.Application.Auth.DTOs;
using SecureFlow.Application.Common.Interfaces;
using System;

namespace SecureFlow.Application.Auth.Login;

public class LoginService
{
    private readonly IAppDbContext _db;
    private readonly IPasswordHasherService _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public LoginService(
        IAppDbContext db,
        IPasswordHasherService hasher,
        IJwtTokenGenerator jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null || !_hasher.Verify(user.PasswordHash, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return new LoginResponseDto
        {
            AccessToken = _jwt.GenerateToken(user),
            RefreshToken = Guid.NewGuid().ToString()
        };
    }
}

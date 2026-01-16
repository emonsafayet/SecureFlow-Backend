using Microsoft.AspNetCore.Identity;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Domain.Auth;

namespace SecureFlow.Infrastructure.Security;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string hash, string password)
    {
        return _hasher.VerifyHashedPassword(null!, hash, password)
               == PasswordVerificationResult.Success;
    }
}

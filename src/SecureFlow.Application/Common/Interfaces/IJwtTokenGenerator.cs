using SecureFlow.Domain.Auth;

namespace SecureFlow.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

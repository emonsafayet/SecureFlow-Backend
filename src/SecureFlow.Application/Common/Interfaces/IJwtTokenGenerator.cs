using SecureFlow.Domain.Entities;

namespace SecureFlow.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

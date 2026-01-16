namespace SecureFlow.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserGuid { get; }
    int UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}

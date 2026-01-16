using Microsoft.AspNetCore.Http;
using SecureFlow.Application.Common.Interfaces;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
     int.Parse(_httpContextAccessor.HttpContext!
         .User.FindFirst("userId")!.Value);

    public Guid? UserGuid =>
        Guid.Parse(_httpContextAccessor.HttpContext!
            .User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public string? Email =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Email)?.Value;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}

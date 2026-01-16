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

    public int UserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User.Identity?.IsAuthenticated != true)
                return 0;

            var userIdClaim = httpContext.User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return 0;

            return int.TryParse(userIdClaim.Value, out var userId) ? userId : 0;
        }
    }

    public Guid? UserGuid
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User.Identity?.IsAuthenticated != true)
                return null;

            var nameIdentifierClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null || string.IsNullOrEmpty(nameIdentifierClaim.Value))
                return null;

            return Guid.TryParse(nameIdentifierClaim.Value, out var userGuid) ? userGuid : null;
        }
    }

    public string? Email =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Email)?.Value;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IReadOnlyCollection<string> Permissions
    {
        get
        {
            return _httpContextAccessor.HttpContext?
            .User?
            .Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .Distinct()
            .ToList() ?? null;
        }
    }
}

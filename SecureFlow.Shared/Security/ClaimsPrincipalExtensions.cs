using System.Security.Claims;

namespace SecureFlow.Shared.Security;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserGuid(this ClaimsPrincipal principal)
        => Guid.TryParse(
            principal.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id) ? id : null;

    public static int GetUserId(this ClaimsPrincipal principal)
        => int.TryParse(
            principal.FindFirstValue(CustomClaims.UserId),
            out var id) ? id : 0;

    public static string? GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.Email);

    public static IEnumerable<string> GetRoles(this ClaimsPrincipal principal)
        => principal.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

    private static string? FindFirstValue(
        this ClaimsPrincipal principal,
        string claimType)
        => principal?.FindFirst(claimType)?.Value;
}

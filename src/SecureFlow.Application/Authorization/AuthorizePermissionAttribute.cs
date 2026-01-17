using Microsoft.AspNetCore.Authorization;

namespace SecureFlow.Application.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class AuthorizePermissionAttribute : AuthorizeAttribute
{
    public AuthorizePermissionAttribute(string action, string resource)
    {
        Policy = $"Permissions.{resource}.{action}";
    }
}
 

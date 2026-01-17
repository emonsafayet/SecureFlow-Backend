using Microsoft.AspNetCore.Authorization;
using SecureFlow.Application.Common.Exceptions;

namespace SecureFlow.Application.Common.Authorization;

public class PermissionHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var hasPermission = context.User
            .Claims
            .Any(c =>
                c.Type == "permission" &&
                c.Value == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else throw new ForbiddenException("Access denied");

        return Task.CompletedTask;
    }
}
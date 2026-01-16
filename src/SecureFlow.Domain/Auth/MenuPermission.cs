using SecureFlow.Shared.Authorization;

namespace SecureFlow.Domain.Auth;

public class MenuPermission
{
    public int MenuId { get; set; }
    public Menu Menu { get; set; } = default!;

    public int PermissionId { get; set; }
    public PermissionEntity Permission { get; set; } = default!;
}
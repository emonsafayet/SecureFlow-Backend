using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth;

public class Role : BaseEntity, IAuthEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RolePermission> RolePermissions { get; set; }
        = new List<RolePermission>();
}

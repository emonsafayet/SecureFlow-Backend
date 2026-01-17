using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth;

public class Menu : AuditableSoftDeleteEntity, IAuditableEntity, IAuthEntity
{
    public string Name { get; set; } = default!;
    public string? Url { get; set; }
    public bool IsActive { get; set; }=true;
    public int Order { get; set; }

    public ICollection<MenuPermission> MenuPermissions { get; set; }
        = new List<MenuPermission>();
}
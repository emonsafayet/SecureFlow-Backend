using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth;

public class Menu : AuditableSoftDeleteEntity, IAuthEntity
{
    public string Name { get; set; } = default!;
    public string? Url { get; set; }
    public int Order { get; set; }

    public ICollection<MenuPermission> MenuPermissions { get; set; }
        = new List<MenuPermission>();
}
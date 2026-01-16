using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth;

public class PermissionEntity : BaseEntity, IAuthEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    public string Action { get; set; } = default!;
    public string Resource { get; set; } = default!;
    public string Description { get; set; } = default!;
}
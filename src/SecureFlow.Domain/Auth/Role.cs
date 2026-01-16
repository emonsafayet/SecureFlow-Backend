using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth;

public class Role : AuditableSoftDeleteEntity, IAuthEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth;

public class UserRole : AuditableSoftDeleteEntity, IAuthEntity
{
    public int UserId { get; set; }     // int FK
    public User User { get; set; } = default!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}

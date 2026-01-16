using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth
{
    public class RolePermission : IAuthEntity
    {
        public int RoleId { get; set; }   
        public int PermissionId { get; set; }

        public Role Role { get; set; } = default!;
        public PermissionEntity Permission { get; set; } = default!;
    }
}

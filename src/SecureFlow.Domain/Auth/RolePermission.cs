using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Auth
{
    public class RolePermission : IAuthEntity
    {
        public int RoleId { get; set; }
        public string Permission { get; set; } = default!;
    }
}

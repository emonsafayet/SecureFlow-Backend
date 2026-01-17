using SecureFlow.Domain.Business.Evidences;
using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;
using SecureFlow.Domain.Enums;

namespace SecureFlow.Domain.Auth;

public class User : AuditableSoftDeleteEntity , IAuditableEntity, IAuthEntity
{
    public Guid UserId { get;  set; } = Guid.NewGuid(); // public-safe ID       
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public int? ProImgEvidenceId { get; set; }
    public Evidence ProImgEvidence { get; set; }
    public UserTypeEnum UserType { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

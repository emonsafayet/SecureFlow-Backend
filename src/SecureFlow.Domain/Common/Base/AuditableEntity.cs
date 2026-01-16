using SecureFlow.Domain.Common.Abstractions;

namespace SecureFlow.Domain.Common.Base;

public abstract class AuditableEntity : BaseEntity, IAuditable
{
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}

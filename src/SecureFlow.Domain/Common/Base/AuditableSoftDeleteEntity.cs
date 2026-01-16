using SecureFlow.Domain.Common.Abstractions;

namespace SecureFlow.Domain.Common.Base;

public abstract class AuditableSoftDeleteEntity
    : AuditableEntity, ISoftDelete
{
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
    public bool IsDeleted => DeletedOn != null;
}

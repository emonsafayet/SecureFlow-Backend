namespace SecureFlow.Domain.Common.Abstractions;

public interface ISoftDelete
{
    DateTime? DeletedOn { get; set; }
    int? DeletedBy { get; set; }
    bool IsDeleted { get; }
}

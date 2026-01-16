namespace SecureFlow.Domain.Common.Abstractions;

public interface IAuditable
{
    int CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
    int? LastModifiedBy { get; set; }
    DateTime? LastModifiedOn { get; set; }
}

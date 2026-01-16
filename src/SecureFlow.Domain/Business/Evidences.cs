using SecureFlow.Domain.Common.Base;
using SecureFlow.Domain.Common.Markers;

namespace SecureFlow.Domain.Business.Evidences;

public class Evidence
    : AuditableSoftDeleteEntity, IBusinessEntity
{
    public string FileName { get; set; } = default!;
    public string FileExtension { get; set; } = default!;
    public long FileSizeInBytes { get; set; }
    public string ContentType { get; set; } = default!;
    public string StorageKey { get; set; } = default!;
    public string StorageBucket { get; set; } = default!;
    public string? FileUrl { get; set; }
    public int UploadedByUserId { get; set; }
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
    public string? Description { get; set; }
}

public class AuditLog
{
    public int Id { get; set; }

    public string EntityName { get; set; } = default!;
    public string EntityId { get; set; } = default!;
    public string Action { get; set; } = default!; // Create | Update | Delete

    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    public int UserId { get; set; }
    public DateTime CreatedOn { get; set; }
}
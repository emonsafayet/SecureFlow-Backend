namespace SecureFlow.Application.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }          // External ID
        public int UserId { get; set; }       // Internal DB ID
        public string Email { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; } = default!;
    }
}

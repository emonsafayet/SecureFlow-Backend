namespace SecureFlow.Application.Menus.DTOs;

public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Url { get; set; }
    public int Order { get; set; }
}
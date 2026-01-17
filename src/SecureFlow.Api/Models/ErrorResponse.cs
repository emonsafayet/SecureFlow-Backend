namespace SecureFlow.API.Models;

public class ErrorResponse
{
    public string Message { get; set; } = default!;
    public object? Errors { get; set; }
}
namespace SecureFlow.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }

    private Result(bool success, T? data, IReadOnlyList<string>? errors)
    {
        IsSuccess = success;
        Data = data;
        Errors = errors;
    }

    public static Result<T> Success(T data)
        => new(true, data, null);

    public static Result<T> Failure(params string[] errors)
        => new(false, default, errors);
}
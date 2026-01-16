namespace SecureFlow.Shared.Models;

public class PaginationResponse<T>
{
    public PaginationResponse(
        IReadOnlyList<T> data,
        int totalCount,
        int pageNumber,
        int pageSize)
    {
        Data = data;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = pageSize > 0
            ? (int)Math.Ceiling(totalCount / (double)pageSize)
            : 0;
    }

    public IReadOnlyList<T> Data { get; }

    public int TotalCount { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalPages { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}

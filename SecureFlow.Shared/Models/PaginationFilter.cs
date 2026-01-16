namespace SecureFlow.Shared.Models;

public class PaginationFilter
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// Optional sorting: e.g. ["name asc", "createdOn desc"]
    /// </summary>
    public string[]? OrderBy { get; set; }

    public bool HasPaging =>
        PageNumber > 0 && PageSize > 0;
}

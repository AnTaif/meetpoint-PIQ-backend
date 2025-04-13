namespace Core.Results;

public class PaginatedResponse<T>(List<T> items, int totalCount, int pageNumber, int pageSize)
{
    public List<T> Items { get; set; } = items;

    public int TotalCount { get; set; } = totalCount;

    public int PageNumber { get; set; } = pageNumber;

    public int PageSize { get; set; } = pageSize;
}
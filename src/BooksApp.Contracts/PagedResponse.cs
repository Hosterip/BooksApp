namespace BooksApp.Contracts;

public class PagedResponse<T>
    where T : class
{
    public required IEnumerable<T> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int TotalPages { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
}
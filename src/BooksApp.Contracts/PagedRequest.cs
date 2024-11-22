namespace BooksApp.Contracts;

public class PagedRequest
{
    public int? Page { get; init; }
    public int? PageSize { get; init; }
}
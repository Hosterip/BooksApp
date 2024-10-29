namespace BooksApp.Contracts.Requests.Books;

public class GetBooksRequest
{
    public int? Limit { get; init; }
    public int? Page { get; init; }
    public string? Q { get; init; }
    public Guid? GenreId { get; init; }
}
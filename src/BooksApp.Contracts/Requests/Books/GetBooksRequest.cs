namespace BooksApp.Contracts.Requests.Books;

public class GetBooksRequest : PagedRequest
{
    public string? Q { get; init; }
    public Guid? GenreId { get; init; }
}
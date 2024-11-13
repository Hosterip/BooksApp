namespace BooksApp.Contracts.Requests.Books;

public class GetBooksRequest : PagedRequest
{
    public string? Title { get; init; }
    public Guid? GenreId { get; init; }
}
namespace BooksApp.Contracts.Requests.Books;

public class GetUserBooksRequest : PagedRequest
{
    public string? Q { get; init; }
    public Guid? GenreId { get; init; }
}
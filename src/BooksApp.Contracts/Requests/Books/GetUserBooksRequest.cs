namespace BooksApp.Contracts.Requests.Books;

public class GetUserBooksRequest : PagedRequest
{
    public string? Title { get; init; }
    public Guid? GenreId { get; init; }
}
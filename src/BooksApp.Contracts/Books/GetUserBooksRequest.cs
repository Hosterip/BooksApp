namespace BooksApp.Contracts.Books;

public class GetUserBooksRequest : PagedRequest
{
    public string? Title { get; init; }
    public Guid? GenreId { get; init; }
}
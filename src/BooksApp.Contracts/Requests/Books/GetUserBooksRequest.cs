namespace BooksApp.Contracts.Requests.Books;

public class GetUserBooksRequest
{
    public required int? Limit { get; init; }
    public required int? Page { get; init; }
    public required string? Q { get; init; }
    public required Guid? GenreId { get; init; }
}
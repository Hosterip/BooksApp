namespace BooksApp.Contracts.Requests.Bookshelves;

public class GetBookshelfBooksRequest
{
    public required int? Limit { get; init; }
    public required int? Page { get; init; }
}
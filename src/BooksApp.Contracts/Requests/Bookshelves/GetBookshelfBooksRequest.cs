namespace BooksApp.Contracts.Requests.Bookshelves;

public class GetBookshelfBooksRequest
{
    public int? Limit { get; init; }
    public int? Page { get; init; }
}
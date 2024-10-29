namespace PostsApp.Common.Contracts.Requests.Book;

public class GetBookshelfBooksRequest
{
    public required int? Limit { get; init; }
    public required int? Page { get; init; }
}
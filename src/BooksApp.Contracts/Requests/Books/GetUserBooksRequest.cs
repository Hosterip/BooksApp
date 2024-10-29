namespace PostsApp.Common.Contracts.Requests.Book;

public class GetUserBooksRequest
{
    public required int? Limit { get; set; }
    public required int? Page { get; set; }
    public required string? Q { get; set; }
    public required Guid? GenreId { get; set; }
}
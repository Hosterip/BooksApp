namespace BooksApp.Contracts.Requests.Users;

public class GetUsersRequest
{
    public required int? Limit { get; init; }
    public required int? Page { get; init; }
    public required string? Q { get; init; }
}
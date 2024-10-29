namespace BooksApp.Contracts.Requests.Users;

public class GetUsersRequest
{
    public int? Limit { get; init; }
    public int? Page { get; init; }
    public string? Q { get; init; }
}
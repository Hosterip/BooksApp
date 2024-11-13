namespace BooksApp.Contracts.Requests.Users;

public class GetFollowersRequest : PagedRequest
{
    public required string? Query { get; init; }
}
namespace BooksApp.Contracts.Users;

public class GetFollowersRequest : PagedRequest
{
    public required string? Query { get; init; }
}
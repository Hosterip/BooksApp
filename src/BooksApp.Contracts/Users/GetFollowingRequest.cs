namespace BooksApp.Contracts.Users;

public class GetFollowingRequest : PagedRequest
{
    public required string? Query { get; init; }
}
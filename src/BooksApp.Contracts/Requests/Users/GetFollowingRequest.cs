namespace BooksApp.Contracts.Requests.Users;

public class GetFollowingRequest : PagedRequest
{
    public required string Query { get; init; }
}
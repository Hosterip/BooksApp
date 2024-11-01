namespace BooksApp.Contracts.Requests.Users;

public class GetUsersRequest : PagedRequest
{
    public string? Q { get; init; }
}
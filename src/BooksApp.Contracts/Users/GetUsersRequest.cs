namespace BooksApp.Contracts.Users;

public class GetUsersRequest : PagedRequest
{
    public string? Q { get; init; }
}
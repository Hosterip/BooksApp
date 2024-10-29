namespace BooksApp.Contracts.Requests.Roles;

public class ChangeRoleRequest
{
    public required Guid UserId { get; init; }
    public required string Role { get; init; }
}
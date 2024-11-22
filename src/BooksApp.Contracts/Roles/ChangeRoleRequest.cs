namespace BooksApp.Contracts.Roles;

public class ChangeRoleRequest
{
    public required Guid UserId { get; init; }
    public required string Role { get; init; }
}
namespace BooksApp.Contracts.Roles;

public sealed class RoleResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
using BooksApp.Application.Common.Results;

namespace BooksApp.Application.Users.Results;

public class UserResult
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string Role { get; init; }
    public required string? AvatarName { get; init; }
    public ViewerRelationship? ViewerRelationship { get; init; }
}
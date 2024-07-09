namespace PostsApp.Application.Users.Results;

public class UserResult
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Role { get; init; }
    public required string? AvatarName { get; init; }
}
namespace PostsApp.Application.Common.Results;

public class UserResult
{
    public required int Id { get; init; }
    public required string Username { get; init; }
    public required string Role { get; init; }
    public required string? AvatarName { get; init; }
}
namespace PostsApp.Application.Common.Results;

public class UserResult
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Role { get; init; }
    public string? AvatarName { get; set; }
}
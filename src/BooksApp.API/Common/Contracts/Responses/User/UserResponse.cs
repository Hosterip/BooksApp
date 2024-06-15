namespace PostsApp.Common.Contracts.Responses.User;

public class UserResponse
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Role { get; init; }

}
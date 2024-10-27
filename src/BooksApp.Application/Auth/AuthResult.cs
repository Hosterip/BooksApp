using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Auth;

public class AuthResult : UserResult
{
    public required string SecurityStamp { get; init; }
}
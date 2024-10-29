using BooksApp.Application.Users.Results;

namespace BooksApp.Application.Auth;

public class AuthResult : UserResult
{
    public required string SecurityStamp { get; init; }
}
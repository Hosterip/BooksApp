namespace PostsApp.Domain.Auth;

public class AuthException : Exception
{
    public AuthException(string message) : base(message)
    {
    }
}
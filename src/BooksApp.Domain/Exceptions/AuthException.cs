namespace PostsApp.Domain.Exceptions;

public class AuthException : Exception
{
    public AuthException(string message) : base(message)
    {
    }
}
namespace PostsApp.Domain.Exceptions;

public class BookException : Exception
{
    public BookException(string message) : base(message) {}
}
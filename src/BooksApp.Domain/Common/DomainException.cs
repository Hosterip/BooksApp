namespace BooksApp.Domain.Common;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
        Message = message;
    }

    public string Message { get; private set; }
}
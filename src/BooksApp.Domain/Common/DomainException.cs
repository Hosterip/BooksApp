namespace BooksApp.Domain.Common;

public class DomainException : Exception
{
    public string Message { get; private set; }
    public DomainException(string message) : base(message)
    {
        Message = message;
    }
}
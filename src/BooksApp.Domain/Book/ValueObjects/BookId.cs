using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Book.ValueObjects;

public class BookId : ValueObject
{
    private BookId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BookId CreateBookId(Guid? value = null)
    {
        return new BookId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
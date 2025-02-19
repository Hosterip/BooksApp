using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Bookshelf.ValueObjects;

public class BookshelfId : ValueObject
{
    private BookshelfId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static BookshelfId CreateBookshelfId(Guid? value = null)
    {
        return new BookshelfId(value ?? Guid.NewGuid());
    }
}
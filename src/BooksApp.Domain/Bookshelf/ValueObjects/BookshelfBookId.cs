using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Bookshelf.ValueObjects;

public class BookshelfBookId : ValueObject
{
    private BookshelfBookId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static BookshelfBookId Create(Guid? value = null)
    {
        return new BookshelfBookId(value ?? Guid.NewGuid());
    }
}
using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Bookshelf.ValueObjects;

public class BookshelfBookId : ValueObject
{
    public BookshelfBookId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static BookshelfBookId CreateBookshelfBookId(Guid? value = null)
    {
        return new BookshelfBookId(value ?? Guid.NewGuid());
    }
}
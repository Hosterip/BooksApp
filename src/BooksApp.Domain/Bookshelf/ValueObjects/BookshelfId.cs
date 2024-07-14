using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Bookshelf.ValueObjects;

public class BookshelfId : ValueObject
{
    public Guid Value { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public BookshelfId(Guid value)
    {
        Value = value;
    }

    public static BookshelfId CreateBookshelfId(Guid? value = null)
    {
        return new(value ?? Guid.NewGuid());
    }
}
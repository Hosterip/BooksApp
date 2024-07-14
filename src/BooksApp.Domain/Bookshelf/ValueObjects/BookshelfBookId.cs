using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Bookshelf.ValueObjects;

public class BookshelfBookId : ValueObject
{
    public Guid Value { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public BookshelfBookId(Guid value)
    {
        Value = value;
    }

    public static BookshelfBookId CreateBookshelfBookId(Guid? value = null)
    {
        return new(value ?? Guid.NewGuid());
    }
}
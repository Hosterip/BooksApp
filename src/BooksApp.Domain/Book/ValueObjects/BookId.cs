using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Book.ValueObjects;

public class BookId : ValueObject
{
    public Guid Value { get; }

    public BookId(Guid value)
    {
        Value = value;
    }

    public static BookId CreateBookId()
    {
        return new(Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
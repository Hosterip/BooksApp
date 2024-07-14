using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Genre.ValueObjects;

public class GenreId : ValueObject
{
    public Guid Value { get; }

    private GenreId(Guid value)
    {
        Value = value;
    }

    public static GenreId CreateGenreId(Guid? value = null)
    {
        return new(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
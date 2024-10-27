using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Genre.ValueObjects;

public class GenreId : ValueObject
{
    private GenreId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static GenreId CreateGenreId(Guid? value = null)
    {
        return new GenreId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
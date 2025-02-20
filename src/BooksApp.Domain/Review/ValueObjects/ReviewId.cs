using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Review.ValueObjects;

public class ReviewId : ValueObject
{
    private ReviewId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ReviewId Create(Guid? value = null)
    {
        return new ReviewId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Review.ValueObjects;

public class ReviewId : ValueObject
{
    public Guid Value { get; }

    public ReviewId(Guid value)
    {
        Value = value;
    }

    public static ReviewId CreateReviewId()
    {
        return new(Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
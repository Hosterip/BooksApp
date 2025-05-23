using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.User.ValueObjects;

public class FollowerId : ValueObject
{
    private FollowerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static FollowerId Create(Guid? value = null)
    {
        return new FollowerId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
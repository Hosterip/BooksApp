using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.User.ValueObjects;

public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        Value = value;
    }

    public static UserId CreateUserId()
    {
        return new(Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
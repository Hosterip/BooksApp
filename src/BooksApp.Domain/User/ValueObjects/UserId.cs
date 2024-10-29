using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.User.ValueObjects;

public sealed class UserId : ValueObject
{
    public UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static UserId CreateUserId(Guid? value = null)
    {
        return new UserId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
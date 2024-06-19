using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Role.ValueObjects;

public class RoleId : ValueObject
{
    public Guid Value { get; }

    public RoleId(Guid value)
    {
        Value = value;
    }

    public static RoleId CreateRoleId(Guid? value = null)
    {
        return new(value ?? Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Role.ValueObjects;

namespace PostsApp.Domain.Role;

public class Role : AggregateRoot<RoleId>
{
    private Role(RoleId id) : base(id) { }

    private Role(
        RoleId id,
        string name
    ) : base(id)
    {
        Name = name;
    }
    public string Name { get; set; }

    public static Role Create(string name)
    {
        return new(RoleId.CreateRoleId(), name);
    }
}
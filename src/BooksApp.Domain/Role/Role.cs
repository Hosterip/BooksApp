using PostsApp.Domain.Common.Constants;
using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Role.ValueObjects;

namespace PostsApp.Domain.Role;

public class Role : AggregateRoot<RoleId>
{
    private Role(RoleId id) : base(id)
    {
    }

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
        return new Role(RoleId.CreateRoleId(), name);
    }

    public static Role Member()
    {
        return new Role(
            RoleId.CreateRoleId(Guid.Parse("6c427f5f-ebc3-4995-8574-53a620a42609")),
            RoleNames.Member);
    }

    public static Role Author()
    {
        return new Role(
            RoleId.CreateRoleId(Guid.Parse("b9ec54dc-01c3-4e43-83a7-b46dc0400102")),
            RoleNames.Author);
    }

    public static Role Moderator()
    {
        return new Role(
            RoleId.CreateRoleId(Guid.Parse("dbffd125-1660-4476-a55d-f2223114120f")),
            RoleNames.Moderator);
    }

    public static Role Admin()
    {
        return new Role(
            RoleId.CreateRoleId(Guid.Parse("509c183d-17ff-4ae4-ace5-b3ed06b09db4")),
            RoleNames.Admin);
    }
}
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Role.ValueObjects;

namespace BooksApp.Domain.Role;

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

    public string Name { get; init; }

    internal static Role Create(string name, Guid? roleId = null)
    {
        return new Role(RoleId.CreateRoleId(roleId), name);
    }
}
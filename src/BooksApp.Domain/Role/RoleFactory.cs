using BooksApp.Domain.Common.Constants;

namespace BooksApp.Domain.Role;

public static class RoleFactory
{
    public static Role Member()
    {
        return Role.Create(
            RoleNames.Member,
            Guid.Parse("6c427f5f-ebc3-4995-8574-53a620a42609"));
    }

    public static Role Author()
    {
        return Role.Create(
            RoleNames.Author,
            Guid.Parse("b9ec54dc-01c3-4e43-83a7-b46dc0400102"));
    }

    public static Role Moderator()
    {
        return Role.Create(
            RoleNames.Moderator,
            Guid.Parse("dbffd125-1660-4476-a55d-f2223114120f"));
    }

    public static Role Admin()
    {
        return Role.Create(
            RoleNames.Admin,
            Guid.Parse("509c183d-17ff-4ae4-ace5-b3ed06b09db4"));
    }
}
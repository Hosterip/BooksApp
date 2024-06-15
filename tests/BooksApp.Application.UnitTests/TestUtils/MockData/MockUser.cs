using PostsApp.Domain.Common.Constants;
using PostsApp.Domain.Image;
using PostsApp.Domain.User;

namespace Application.UnitTest.TestUtils.MockData;

public static class MockUser
{
    public static User GetUser(
        string role = RoleNames.Member,
        string? hash = null,
        string? salt = null) => User.Create(
        "Hello world",
        MockRole.GetRole(role ?? RoleNames.Member),
        hash ?? "Hello world",
        salt ?? "Hello world",
        Image.Create("hello world"));
}
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Image;
using BooksApp.Domain.User;

namespace Application.UnitTest.TestUtils.MockData;

public static class MockUser
{
    public static User GetUser(
        string role = RoleNames.Member,
        string? hash = null,
        string? salt = null)
    {
        return User.Create(
            "Hello world",
            "Hello world",
            "Hello world",
            "Hello world",
            MockRole.GetRole(role ?? RoleNames.Member),
            hash ?? "Hello world",
            salt ?? "Hello world",
            Image.Create("hello world"));
    }
}
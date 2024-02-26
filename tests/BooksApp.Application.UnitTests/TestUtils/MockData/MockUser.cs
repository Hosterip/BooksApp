using PostsApp.Domain.Constants;
using PostsApp.Domain.Models;

namespace Application.UnitTest.TestUtils.MockData;

public static class MockUser
{
    public static User GetUser(
        string? role = null,
        string? hash = null,
        string? salt = null) => new User
    {
        Id = 0,
        Username = "Hello world",
        Hash = hash ?? "Hello world",
        Salt = salt ?? "Hello world",
        Role = MockRole.GetRole(role ?? RoleConstants.Member)
    };
}
using PostsApp.Domain.Constants;
using PostsApp.Domain.Models;

namespace Application.UnitTest.TestUtils.MockData;

public static class MockUser
{
    public static User GetUser(string? role) => new User
    {
        Id = 0,
        Username = "Hello world",
        Hash = "Hello world",
        Salt = "Hello world",
        Role = MockRole.GetRole(role ?? RoleConstants.Member)
    };
}
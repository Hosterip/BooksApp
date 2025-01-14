using BooksApp.Domain.Common.Interfaces;
using BooksApp.Infrastructure.Authentication;
using NSubstitute;

namespace TestCommon.Common.Factories;

public static class PasswordHasherFactory
{
    public static IPasswordHasher CreatePasswordHasher()
    {
        var mockPasswordHasher = Substitute.For<IPasswordHasher>();
        mockPasswordHasher.GenerateHashSalt(string.Empty).ReturnsForAnyArgs(("foo", "bar"));
        return new PasswordHasher();
    }
}
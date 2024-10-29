using Application.UnitTest.Auth.TestUtils.Constants;
using BooksApp.Application.Auth.Queries.Login;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class AuthQueriesUtils
{
    public static LoginUserQuery LoginUserQueryCorrect =>
        new()
        {
            Email = "mock",
            Password = PasswordConstants.CorrectPassword
        };

    public static LoginUserQuery LoginUserQueryIncorrect =>
        new()
        {
            Email = "mock",
            Password = PasswordConstants.IncorrectPassword
        };
}
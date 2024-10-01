using Application.UnitTest.Auth.TestUtils.Constants;
using PostsApp.Application.Auth.Queries.Login;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class AuthQueriesUtils
{
    public static LoginUserQuery LoginUserQueryCorrect => 
        new LoginUserQuery
        {
            Email = "mock", 
            Password = PasswordConstants.CorrectPassword
        };
    public static LoginUserQuery LoginUserQueryIncorrect => 
        new LoginUserQuery
        {
            Email = "mock",
            Password = PasswordConstants.IncorrectPassword
        };
}
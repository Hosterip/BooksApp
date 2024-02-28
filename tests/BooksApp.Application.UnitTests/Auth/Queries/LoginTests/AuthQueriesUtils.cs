using Application.UnitTest.Auth.TestUtils.Constants;
using PostsApp.Application.Auth.Queries.Login;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class AuthQueriesUtils
{
    public static LoginUserQuery LoginUserQueryCorrect => 
        new LoginUserQuery
        {
            Username = "mock", 
            Password = PasswordConstants.CorrectPassword
        };
    public static LoginUserQuery LoginUserQueryIncorrect => 
        new LoginUserQuery
        {
            Username = "mock",
            Password = PasswordConstants.IncorrectPassword
        };
}
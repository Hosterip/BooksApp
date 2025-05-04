using BooksApp.Application.Auth.Queries.Login;
using TestCommon.Common.Constants;

namespace TestCommon.Auth;

public static class AuthQueryFactory
{
    public static LoginUserQuery CreateLoginUserQuery(
        string email = Constants.Users.Email,
        string password = Constants.Users.Password)
    {
        return new LoginUserQuery {
            Email = email,
            Password = password
        };
    }
}
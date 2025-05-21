using Bogus;
using BooksApp.Application.Auth.Queries.Login;
using TestCommon.Common.Constants;

namespace TestCommon.Auth;

public static class AuthQueryFactory
{
    public static LoginUserQuery CreateLoginUserQuery(
        string? email = null,
        string? password = null)
    {
        return new Faker<LoginUserQuery>()
            .RuleFor(x => x.Email, f => email ?? f.Person.Email)
            .RuleFor(x => x.Password, f => password ?? f.Lorem.Slug());
    }
}
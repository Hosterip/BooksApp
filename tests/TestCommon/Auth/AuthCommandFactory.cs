using Bogus;
using BooksApp.Application.Auth.Commands.ChangePassword;
using BooksApp.Application.Auth.Commands.Register;
using TestCommon.Common.Constants;

namespace TestCommon.Auth;

public static class AuthCommandFactory
{
    public static ChangePasswordCommand CreateChangePasswordCommand(
        string? oldPassword = null,
        string? newPassword = null)
    {
        return new Faker<ChangePasswordCommand>()
            .RuleFor(x => x.OldPassword, f => oldPassword ?? f.Lorem.Slug())
            .RuleFor(x => x.NewPassword, f => newPassword ?? f.Lorem.Slug());
    }

    public static RegisterUserCommand CreateRegisterUserCommand(
        string? email = null,
        string? firstName = null,
        string? middleName = null,
        string? lastName = null,
        string? password = null)
    {
        return new Faker<RegisterUserCommand>()
            .RuleFor(x => x.Email, f => email ?? f.Person.Email)
            .RuleFor(x => x.FirstName, f => firstName ?? f.Person.FirstName)
            .RuleFor(x => x.MiddleName, f => middleName ?? f.Person.FirstName)
            .RuleFor(x => x.LastName, f => lastName ?? f.Person.LastName)
            .RuleFor(x => x.Password, f => password ?? f.Lorem.Slug());
    }
}
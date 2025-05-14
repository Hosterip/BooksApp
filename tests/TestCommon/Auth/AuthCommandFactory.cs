using BooksApp.Application.Auth.Commands.ChangePassword;
using BooksApp.Application.Auth.Commands.Register;
using TestCommon.Common.Constants;

namespace TestCommon.Auth;

public static class AuthCommandFactory
{
    public static ChangePasswordCommand CreateChangePasswordCommand(
        string oldPassword = Constants.Users.Password,
        string newPassword = Constants.Users.Password + "foo")
    {
        return new ChangePasswordCommand
        {
            OldPassword = oldPassword,
            NewPassword = newPassword
        };
    }

    public static RegisterUserCommand CreateRegisterUserCommand(
        string email = Constants.Users.Email,
        string firstName = Constants.Users.FirstName,
        string middleName = Constants.Users.LastName,
        string lastName = Constants.Users.LastName,
        string password = Constants.Users.Password)
    {
        return new RegisterUserCommand
        {
            Email = email,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            Password = password
        };
    }
}
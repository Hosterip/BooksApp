using Application.UnitTest.Auth.TestUtils.Constants;
using BooksApp.Application.Auth.Commands.ChangePassword;

namespace Application.UnitTest.Auth.Commands.TestUtils;

public static class AuthCommandsUtils
{
    public static ChangePasswordCommand ChangePasswordCommandCorrect =>
        new()
        {
            Id = new Guid("1"),
            NewPassword = PasswordConstants.MockPassword,
            OldPassword = PasswordConstants.CorrectPassword
        };

    public static ChangePasswordCommand ChangePasswordCommandIncorrect =>
        new()
        {
            Id = new Guid("1"),
            NewPassword = PasswordConstants.MockPassword,
            OldPassword = PasswordConstants.IncorrectPassword
        };
}
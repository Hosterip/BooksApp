using Application.UnitTest.Auth.TestUtils.Constants;
using PostsApp.Application.Auth.Commands.ChangePassword;

namespace Application.UnitTest.Auth.Commands.TestUtils;

public static class AuthCommandsUtils
{
    public static ChangePasswordCommand ChangePasswordCommandCorrect => 
        new ChangePasswordCommand
        {
            Id = new Guid("1"),
            NewPassword = PasswordConstants.MockPassword,
            OldPassword = PasswordConstants.CorrectPassword
        };
    public static ChangePasswordCommand ChangePasswordCommandIncorrect => 
        new ChangePasswordCommand
        {
            Id = new Guid("1"),
            NewPassword = PasswordConstants.MockPassword,
            OldPassword = PasswordConstants.IncorrectPassword
        };
    
}
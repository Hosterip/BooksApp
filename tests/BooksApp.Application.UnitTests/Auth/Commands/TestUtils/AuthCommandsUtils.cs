using PostsApp.Application.Auth.Commands.ChangePassword;

namespace Application.UnitTest.Books.Commands.CreateBookCommandTests.TestUtils;

public static class AuthCommandsUtils
{
    public static ChangePasswordCommand ChangePasswordCommand => 
        new ChangePasswordCommand
        {
            Id = 1,
            NewPassword = "1",
            OldPassword = "2"
        };
}
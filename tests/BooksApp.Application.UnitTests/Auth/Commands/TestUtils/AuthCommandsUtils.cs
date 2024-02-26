using System.Linq.Expressions;
using Application.UnitTest.Auth.TestUtils.Constants;
using Application.UnitTest.TestUtils.MockData;
using Moq;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace Application.UnitTest.Auth.Commands.TestUtils;

public static class AuthCommandsUtils
{
    public static ChangePasswordCommand ChangePasswordCommandCorrect => 
        new ChangePasswordCommand
        {
            Id = 1,
            NewPassword = PasswordConstants.MockPassword,
            OldPassword = PasswordConstants.CorrectPassword
        };
    public static ChangePasswordCommand ChangePasswordCommandIncorrect => 
        new ChangePasswordCommand
        {
            Id = 1,
            NewPassword = PasswordConstants.MockPassword,
            OldPassword = PasswordConstants.IncorrectPassword
        };

    public static void SetupUsersGetSingleWhereAsync(Mock<IUnitOfWork> mockUnitOfWork)
    {
        mockUnitOfWork.Setup(x => x.Users.GetSingleWhereAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(MockUser.GetUser(null, HashSaltConstants.Hash, HashSaltConstants.Salt));
    }
}
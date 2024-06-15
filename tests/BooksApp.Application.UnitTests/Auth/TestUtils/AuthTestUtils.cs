using System.Linq.Expressions;
using Application.UnitTest.Auth.TestUtils.Constants;
using Application.UnitTest.TestUtils.MockData;
using Moq;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.User;

namespace Application.UnitTest.Auth.TestUtils;

public static class AuthTestUtils
{
    public static void SetupUsersAnyAsyncMethod(Mock<IUnitOfWork> mockUnitOfWork, bool user)
    {
        mockUnitOfWork.Setup(x => x.Users.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
    }
    
    public static void SetupUsersGetSingleWhereAsync(Mock<IUnitOfWork> mockUnitOfWork)
    {
        mockUnitOfWork.Setup(x => x.Users.GetSingleWhereAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(MockUser.GetUser(null, HashSaltConstants.Hash, HashSaltConstants.Salt));
    }
}
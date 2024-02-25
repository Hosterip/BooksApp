using System.Linq.Expressions;
using Moq;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace Application.UnitTest.Auth.TestUtils;

public static class AuthTestUtils
{
    public static void SetupUsersAnyAsyncMethod(Mock<IUnitOfWork> mockUnitOfWork, bool user)
    {
        mockUnitOfWork.Setup(x => x.Users.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
    }
}
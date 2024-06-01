using Application.UnitTest.Auth.TestUtils;
using FluentAssertions;
using Moq;
using PostsApp.Application.Auth;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class LoginQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public LoginQueryTests()
    {
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Success_ReturnAuthResult()
    {
        // Arrange
        // All the work with validating password has been made through validator. Here we just returning user
        var query = AuthQueriesUtils.LoginUserQueryCorrect;
        var handler = new LoginUserQueryHandler(_unitOfWorkMock.Object);
        AuthTestUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var result = await handler.Handle(query, default);
        
        // Assert
        result.Should().BeOfType<UserResult>();
    }
}
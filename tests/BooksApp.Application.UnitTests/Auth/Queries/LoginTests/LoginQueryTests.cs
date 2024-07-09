using Application.UnitTest.Auth.TestUtils;
using FluentAssertions;
using MapsterMapper;
using Moq;
using PostsApp.Application.Auth;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class LoginQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapper;
    public LoginQueryTests()
    {
        _unitOfWorkMock = new();
        _mapper = new();
    }

    [Fact]
    public async Task Handle_Success_ReturnAuthResult()
    {
        // Arrange
        // All the work with validating password has been made through validator. Here we just returning user
        var query = AuthQueriesUtils.LoginUserQueryCorrect;
        var handler = new LoginUserQueryHandler(_unitOfWorkMock.Object, _mapper.Object);
        AuthTestUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var result = await handler.Handle(query, default);
        
        // Assert
        result.Should().BeOfType<UserResult>();
    }
}
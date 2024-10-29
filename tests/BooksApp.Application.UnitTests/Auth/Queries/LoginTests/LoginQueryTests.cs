using Application.UnitTest.Auth.TestUtils;
using BooksApp.Application.Auth.Queries.Login;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Results;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class LoginQueryTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public LoginQueryTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapper = new Mock<IMapper>();
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
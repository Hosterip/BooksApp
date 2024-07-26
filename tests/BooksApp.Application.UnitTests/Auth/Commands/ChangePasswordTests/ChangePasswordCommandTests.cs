using Application.UnitTest.Auth.Commands.TestUtils;
using Application.UnitTest.Auth.TestUtils;
using FluentAssertions;
using MapsterMapper;
using Moq;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Common.Interfaces;

namespace Application.UnitTest.Auth.Commands.ChangePasswordTests;

public class ChangePasswordCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapper;

    public ChangePasswordCommandTests()
    {
        _unitOfWorkMock = new();
        _mapper = new();
    }

    [Fact]
    public async Task Handle_CorrectPassword_ReturnVoid()
    {
        // Arrange
        var command = AuthCommandsUtils.ChangePasswordCommandCorrect;
        var handler = new ChangePasswordCommandHandler(_unitOfWorkMock.Object, _mapper.Object);
        AuthTestUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var exception = await Record.ExceptionAsync(() => handler.Handle(command, default));
        
        // Assert
        exception.Should().BeNull();
    }
    
    [Fact]
    public async Task Handle_IncorrectPassword_ReturnVoid()
    {
        // Arrange
        var command = AuthCommandsUtils.ChangePasswordCommandIncorrect;
        var handler = new ChangePasswordCommandHandler(_unitOfWorkMock.Object, _mapper.Object);
        AuthTestUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var exception = await Record.ExceptionAsync(() => handler.Handle(command, default));
        
        // Assert
        exception.Should().NotBeNull();
    }
}
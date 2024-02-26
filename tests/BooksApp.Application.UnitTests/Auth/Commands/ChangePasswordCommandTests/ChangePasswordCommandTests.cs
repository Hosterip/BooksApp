using Application.UnitTest.Auth.Commands.TestUtils;
using FluentAssertions;
using Moq;
using PostsApp.Application.Auth;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Exceptions;

namespace Application.UnitTest.Auth.Commands.ChangePasswordCommandTests;

public class ChangePasswordCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public ChangePasswordCommandTests()
    {
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_CorrectPassword_ReturnAuthResult()
    {
        // Arrange
        var command = AuthCommandsUtils.ChangePasswordCommandCorrect;
        var handler = new ChangePasswordCommandHandler(_unitOfWorkMock.Object);
        AuthCommandsUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var exception = await Record.ExceptionAsync(() => handler.Handle(command, default));
        
        // Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public async Task Handle_IncorrectPassword_ReturnAuthResult()
    {
        // Arrange
        var command = AuthCommandsUtils.ChangePasswordCommandIncorrect;
        var handler = new ChangePasswordCommandHandler(_unitOfWorkMock.Object);
        AuthCommandsUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var exception = await Record.ExceptionAsync(() => handler.Handle(command, default));
        
        // Assert
        Assert.NotNull(exception);
    }
}
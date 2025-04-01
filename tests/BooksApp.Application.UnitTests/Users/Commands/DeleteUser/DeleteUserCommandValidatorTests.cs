using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.DeleteUser;
using FluentAssertions;
using NSubstitute;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Users.Commands.DeleteUser;

public class DeleteUserCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    public DeleteUserCommandValidatorTests()
    {
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var command = UserCommandFactory.CreateDeleteUserCommand();
        var validator = new DeleteUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);    

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserIsNotFound_ShouldReturnSpecificResult()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(false);
        
        var command = UserCommandFactory.CreateDeleteUserCommand();
        var validator = new DeleteUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);    

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.User.NotFound &&
            x.PropertyName == nameof(DeleteUserCommand.UserId));
    }
}
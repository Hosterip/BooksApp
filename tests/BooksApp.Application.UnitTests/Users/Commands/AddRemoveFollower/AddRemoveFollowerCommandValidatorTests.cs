using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.AddRemoveFollower;
using FluentAssertions;
using NSubstitute;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Users.Commands.AddRemoveFollower;

public class AddRemoveFollowerCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public AddRemoveFollowerCommandValidatorTests()
    {
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var command = UserCommandFactory.CreateAddRemoveFollowerCommand();
        var validator = new AddRemoveFollowerCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenTargetUserIsNotFound_ShouldReturnSpecificErrorResult()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        _unitOfWork.Users.AnyById(user.Id.Value).Returns(false);
        
        var command = UserCommandFactory.CreateAddRemoveFollowerCommand(userId: user.Id.Value);
        var validator = new AddRemoveFollowerCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should().ContainSingle(x => 
            x.ErrorMessage == ValidationMessages.User.NotFound &&
            x.PropertyName == nameof(AddRemoveFollowerCommand.UserId));
    }
    
    [Fact]
    public async Task ValidateAsync_When_ShouldReturnSpecificErrorResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userService.GetId().ReturnsForAnyArgs(userId); 
        var command = UserCommandFactory.CreateAddRemoveFollowerCommand(userId: userId);
        var validator = new AddRemoveFollowerCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should().ContainSingle(x => 
            x.ErrorMessage == ValidationMessages.User.CantFollowYourself &&
            x.PropertyName == "followerId");
    }
}
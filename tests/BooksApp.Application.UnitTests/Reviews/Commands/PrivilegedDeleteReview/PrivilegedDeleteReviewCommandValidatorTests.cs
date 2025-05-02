using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;
using BooksApp.Domain.Role;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Reviews;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Reviews.Commands.PrivilegedDeleteReview;

public class PrivilegedDeleteReviewCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public PrivilegedDeleteReviewCommandValidatorTests()
    {
        var user = UserFactory.CreateUser(role: RoleFactory.Admin());

        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _unitOfWork.Users.GetSingleById(Guid.Empty).ReturnsForAnyArgs(user);
        
        _unitOfWork.Reviews.AnyById(Guid.Empty).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldBeValid()
    {
        // Arrange
        //  Creating command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();
        
        //  Creating validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenModeratorDeletesAReview_ShouldBeValid()
    {
        // Arrange
        var user = UserFactory.CreateUser(role: RoleFactory.Moderator());

        _unitOfWork.Users.GetSingleById(Guid.Empty).ReturnsForAnyArgs(user);
        
        //  Creating command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();
        
        //  Creating validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoReview_ShouldReturnSpecificError()
    {
        // Arrange
        //  Making Reviews' AnyById method to return false
        _unitOfWork.Reviews.AnyById(Guid.Empty).ReturnsForAnyArgs(false);
        
        //  Creating command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();
        
        //  Creating validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.PropertyName == nameof(PrivilegedDeleteReviewCommand.ReviewId) &&
            x.ErrorMessage == ValidationMessages.Review.NotFound);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserHasNoPermissionToDelete_ShouldReturnSpecificError()
    {
        // Arrange
        //  Making user
        var user = UserFactory.CreateUser(role: RoleFactory.Member());

        _unitOfWork.Users.GetSingleById(Guid.Empty).ReturnsForAnyArgs(user);
        
        //  Creating command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();
        
        //  Creating validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.PropertyName == nameof(UserId) &&
            x.ErrorMessage == ValidationMessages.User.Permission);
    }
}
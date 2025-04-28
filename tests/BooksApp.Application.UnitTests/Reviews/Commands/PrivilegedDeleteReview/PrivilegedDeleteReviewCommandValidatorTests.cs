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

        _unitOfWork.Reviews.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.GetSingleById(default).ReturnsForAnyArgs(user);
        _userService.GetId().ReturnsForAnyArgs(user.Id.Value);
    }

    [Fact]
    public async Task ValidateAsync_WhenReviewDeletedByAdmin_ShouldReturnAValidResult()
    {
        // Arrange
        //  Create command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();

        //  Create validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenReviewDeletedByModerator_ShouldReturnAValidResult()
    {
        // Arrange
        //  Making GetSingleById return a moderator
        var user = UserFactory.CreateUser(role: RoleFactory.Moderator());

        _unitOfWork.Users.GetSingleById(default).ReturnsForAnyArgs(user);

        //  Create command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();

        //  Create validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenReviewDeletedByAUserWithNotAllowedRole_ShouldReturnAnInvalidResult()
    {
        // Arrange
        //  Making GetSingleById return a member
        var user = UserFactory.CreateUser(role: RoleFactory.Member());

        _unitOfWork.Users.GetSingleById(default).ReturnsForAnyArgs(user);

        //  Create command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();

        //  Create validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(UserId) &&
            x.ErrorMessage == ValidationMessages.User.Permission);
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsNoReview_ShouldReturnAnInvalidResult()
    {
        // Arrange
        //  Making ReviewRepository AnyById method return false
        _unitOfWork.Reviews.AnyById(default).ReturnsForAnyArgs(false);

        //  Create command
        var command = ReviewCommandFactory.CreatePrivilegedDeleteReviewCommand();

        //  Create validator
        var validator = new PrivilegedDeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(PrivilegedDeleteReviewCommand.ReviewId) &&
            x.ErrorMessage == ValidationMessages.Review.NotFound);
    }
}
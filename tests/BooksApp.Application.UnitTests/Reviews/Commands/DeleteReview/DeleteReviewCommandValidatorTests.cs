using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Commands.DeleteReview;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Reviews;

namespace BooksApp.Application.UnitTests.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public DeleteReviewCommandValidatorTests()
    {
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _unitOfWork.Reviews.AnyAsync(default!).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        //  Creating a command
        var command = ReviewCommandFactory.CreateDeleteReviewCommand();

        //  Creating validator
        var validator = new DeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenThereUserDoesNotOwnAReview_ShouldReturnInvalidResult()
    {
        // Arrange
        //  Making AnyAsync method return false
        _unitOfWork.Reviews.AnyAsync(default!).ReturnsForAnyArgs(false);

        //  Creating a command
        var command = ReviewCommandFactory.CreateDeleteReviewCommand();

        //  Creating validator
        var validator = new DeleteReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(UserId) &&
            x.ErrorMessage == ValidationMessages.User.Permission);
    }
}
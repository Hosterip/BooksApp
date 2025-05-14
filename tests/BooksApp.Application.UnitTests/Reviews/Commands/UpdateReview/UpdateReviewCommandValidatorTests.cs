using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Commands.UpdateReview;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Common.Helpers;
using TestCommon.Reviews;

namespace BooksApp.Application.UnitTests.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UpdateReviewCommandValidatorTests()
    {
        var review = ReviewFactory.CreateReview();
        _userService.GetId().ReturnsForAnyArgs(review.User.Id.Value);
        _unitOfWork.Reviews.GetSingleById(Guid.Empty).ReturnsForAnyArgs(review);

        _unitOfWork.Reviews.AnyById(Guid.Empty).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        //  Creating command
        var command = ReviewCommandFactory.CreateUpdateReviewCommand();

        //  Creating validator
        var validator = new UpdateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Review.Body + 1)]
    public async Task ValidateAsync_WhenBodyExceedsAppropriateLength_ShouldReturnAnInvalidResult(int bodyLength)
    {
        // Arrange
        //  Creating body according to bodyLength param
        var body = StringUtilities.GenerateLongString(bodyLength);

        // Arrange
        //  Creating command
        var command = ReviewCommandFactory.CreateUpdateReviewCommand(body: body);

        //  Creating validator
        var validator = new UpdateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(UpdateReviewCommand.Body));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task ValidateAsync_WhenRatingExceedsItsLowestAndMax_ShouldReturnAnInvalidResult(int rating)
    {
        // Arrange
        //  Creating command
        var command = ReviewCommandFactory.CreateUpdateReviewCommand(rating);

        //  Creating validator
        var validator = new UpdateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(UpdateReviewCommand.Rating));
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsNoReview_ShouldReturnAnInvalidResult()
    {
        // Arrange
        //  Making ReviewRepository AnyById method return false
        _unitOfWork.Reviews.AnyById(default).ReturnsForAnyArgs(false);

        //  Creating command
        var command = ReviewCommandFactory.CreateUpdateReviewCommand();

        //  Creating validator
        var validator = new UpdateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(UpdateReviewCommand.ReviewId) &&
            x.ErrorMessage == ValidationMessages.Review.NotFound);
    }

    [Fact]
    public async Task ValidateAsync_WhenReviewDoesNotBelongToUser_ShouldReturnAnInvalidResult()
    {
        // Arrange
        //  Making UserService GetId method return wrong id
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());

        //  Creating command
        var command = ReviewCommandFactory.CreateUpdateReviewCommand();

        //  Creating validator
        var validator = new UpdateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(UserId) &&
            x.ErrorMessage == ValidationMessages.Review.NotYours);
    }
}
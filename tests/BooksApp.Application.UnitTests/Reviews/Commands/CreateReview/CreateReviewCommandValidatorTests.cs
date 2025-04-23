using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Commands.CreateReview;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Common.Helpers;
using TestCommon.Reviews;

namespace BooksApp.Application.UnitTests.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public CreateReviewCommandValidatorTests()
    {
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Reviews.AnyAsync(default!).ReturnsForAnyArgs(false);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOk_ShouldReturnAValidResult()
    {
        // Arrange
        // Creating a command
        var command = ReviewCommandFactory.CreateCreateReviewCommand();

        // Creating a validator
        var validator = new CreateReviewCommandValidator(_unitOfWork, _userService);

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
        
        // Creating a command
        var command = ReviewCommandFactory.CreateCreateReviewCommand(body: body);

        // Creating a validator
        var validator = new CreateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(CreateReviewCommand.Body));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task ValidateAsync_WhenRatingExceedsItsLowestAndMax_ShouldReturnAnInvalidResult(int rating)
    {
        // Arrange
        // Creating a command
        var command = ReviewCommandFactory.CreateCreateReviewCommand(rating: rating);

        // Creating a validator
        var validator = new CreateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(CreateReviewCommand.Rating));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenBookWasNotFound_ShouldReturnAnInvalidResult()
    {
        // Arrange
        // Making BooksRepository AnyById method return false
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(false);
        
        // Creating a command
        var command = ReviewCommandFactory.CreateCreateReviewCommand();

        // Creating a validator
        var validator = new CreateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.PropertyName == nameof(CreateReviewCommand.BookId) && 
            x.ErrorMessage == ValidationMessages.Book.NotFound);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenReviewAlreadyExists_ShouldReturnAnInvalidResult()
    {
        // Arrange
        // Making ReviewsRepository AnyAsync method return true
        _unitOfWork.Reviews.AnyAsync(default!).ReturnsForAnyArgs(true);
        
        // Creating a command
        var command = ReviewCommandFactory.CreateCreateReviewCommand();

        // Creating a validator
        var validator = new CreateReviewCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.PropertyName == nameof(CreateReviewCommand.BookId) && 
            x.ErrorMessage == ValidationMessages.Review.AlreadyHave);
    }
}
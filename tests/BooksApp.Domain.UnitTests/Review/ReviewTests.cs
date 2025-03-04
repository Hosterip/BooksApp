using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using TestCommon.Books;
using TestCommon.Common.Constants;
using TestCommon.Common.Helpers;
using TestCommon.Users;

namespace BooksApp.Domain.UnitTests.Review;

public class ReviewTests
{
    [Fact]
    public void Create_WhenEverythingIsInOrder_ShouldCreateReview()
    {
        // Arrange
        var book = BookFactory.CreateBook();
        var user = UserFactory.CreateUser();
        
        // Act
        var result = Domain.Review.Review.Create(
            Constants.Reviews.Rating,
            Constants.Reviews.Body,
            user,
            book);

        // Assert
        result.Should().BeOfType<Domain.Review.Review>();
        result.Rating.Should().Be(Constants.Reviews.Rating);
    }
    
    [Theory]
    [InlineData(6)]
    [InlineData(0)]
    [InlineData(100)]
    public void Create_WhenRatingIsWrong_ShouldThrowAnError(int rating)
    {
        // Arrange
        var book = BookFactory.CreateBook();
        var user = UserFactory.CreateUser();
        
        // Act
        var act = () => Domain.Review.Review.Create(
            rating,
            Constants.Reviews.Body,
            user,
            book);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Review.Body + 1)]
    public void Create_WhenBodyExceedsMaximumLength_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var book = BookFactory.CreateBook();
        var user = UserFactory.CreateUser();

        var body = StringUtilities.GenerateLongString(stringLength);
        
        // Act
        var act = () => Domain.Review.Review.Create(
            Constants.Reviews.Rating,
            body, 
            user,
            book);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.Review.Body)]
    public void Create_WhenBodyIsWhiteSpace_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var book = BookFactory.CreateBook();
        var user = UserFactory.CreateUser();

        var body = StringUtilities.GenerateLongWhiteSpace(stringLength);
        
        // Act
        var act = () => Domain.Review.Review.Create(
            Constants.Reviews.Rating,
            body, 
            user,
            book);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
}
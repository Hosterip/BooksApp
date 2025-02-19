using BooksApp.Domain.Common;
using FluentAssertions;
using TestCommon.Books;
using TestCommon.Common.Constants;
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
    
    [Fact]
    public void Create_WhenBodyIsWrong_ShouldThrowAnError()
    {
        // Arrange
        var book = BookFactory.CreateBook();
        var user = UserFactory.CreateUser();
        
        // Act
        var act = () => Domain.Review.Review.Create(
            Constants.Reviews.Rating,
            string.Empty, 
            user,
            book);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
}
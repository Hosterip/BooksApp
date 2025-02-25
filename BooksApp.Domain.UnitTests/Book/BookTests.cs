using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using TestCommon.Common.Constants;
using TestCommon.Common.Helpers;
using TestCommon.Genres;
using TestCommon.Images;
using TestCommon.Users;

namespace BooksApp.Domain.UnitTests.Book;

public class BookTests
{
    [Fact]
    public void Create_WhenEverythingInOrder_ShouldCreateBook()
    {
        // Arrange
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();
        
        // Act
        var result = Domain.Book.Book.Create(
            Constants.Books.Title,
            Constants.Books.Description,
            image,
            user,
            genres);

        // Assert
        result.Should().BeOfType<Domain.Book.Book>();
        result.Title.Should().Be(Constants.Books.Title);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Title)]
    public void Create_WhenTitleIsWrong_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();
        var title = StringUtilities.ExceedMaxStringLength(stringLength);
        
        // Act
        var act = () => Domain.Book.Book.Create(
            title,
            Constants.Books.Description,
            image,
            user,
            genres);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Fact]
    public void Create_WhenGenreIsWrong_ShouldThrowAnError()
    {
        // Arrange
        var genres = new List<Genre.Genre>();
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();
        
        // Act
        var act = () => Domain.Book.Book.Create(
            Constants.Books.Title,
            Constants.Books.Description,
            image,
            user,
            genres);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Description)]
    public void Create_WhenDescriptionIsWrong_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var genres = new List<Genre.Genre>();
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();

        var title = StringUtilities.ExceedMaxStringLength(stringLength);
        
        // Act
        var act = () => Domain.Book.Book.Create(
            title,
            string.Empty,
            image,
            user,
            genres);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
}
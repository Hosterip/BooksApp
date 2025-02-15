using BooksApp.Domain.Common;
using FluentAssertions;
using TestCommon.Common.Constants;
using TestCommon.Genres;
using TestCommon.Images;
using TestCommon.Users;

namespace BooksApp.Domain.UnitTests.Books;

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
        var result = Book.Book.Create(
            Constants.Books.Title,
            Constants.Books.Description,
            image,
            user,
            genres);

        // Assert
        result.Should().BeOfType<Book.Book>();
        result.Title.Should().Be(Constants.Books.Title);
    }
    
    [Fact]
    public void Create_WhenEverythingTitleIsWrong_ShouldThrowAnError()
    {
        // Arrange
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();
        
        // Act
        var act = () => Book.Book.Create(
            string.Empty,
            Constants.Books.Description,
            image,
            user,
            genres);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
}
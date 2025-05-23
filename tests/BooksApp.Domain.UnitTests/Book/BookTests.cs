using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using TestCommon.Books;
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
        // Act
        var result = BookFactory.CreateBook(); 

        // Assert
        result.Should().BeOfType<Domain.Book.Book>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Title + 1)]
    public void Create_WhenTitleIsTooLongOrTooShort_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var title = StringUtilities.GenerateLongString(stringLength);

        // Act
        var act = () => BookFactory.CreateBook(title);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage($"Title should be inclusively between 1 and {MaxPropertyLength.Book.Title}");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.Book.Title)]
    public void Create_WhenTitleIsWhitespace_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var title = StringUtilities.GenerateLongWhiteSpace(stringLength);

        // Act
        var act = () => BookFactory.CreateBook(title);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Title should be present and not be white space");
    }

    [Fact]
    public void Create_WhenGenreArrayIsEmpty_ShouldThrowAnError()
    {
        // Arrange
        var genres = new List<Genre.Genre>();

        // Act
        var act = () => BookFactory.CreateBook(genres:genres);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Book must have at least one genre");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Description + 1)]
    public void Create_WhenDescriptionIsTooLongOrTooShort_ShouldThrowAnError(int descriptionLength)
    {
        // Arrange
        var description = StringUtilities.GenerateLongString(descriptionLength);

        // Act
        var act = () => BookFactory.CreateBook(description:description);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage($"Description should be inclusively between 1 and {MaxPropertyLength.Book.Description}");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.Book.Description)]
    public void Create_WhenDescriptionIsWhitespace_ShouldThrowAnError(int descriptionLength)
    {
        // Arrange
        var description = StringUtilities.GenerateLongWhiteSpace(descriptionLength);

        // Act
        var act = () => BookFactory.CreateBook(description:description);
        
        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Description should be present and not be white space");
    }
}
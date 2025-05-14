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
    [InlineData(MaxPropertyLength.Book.Title + 1)]
    public void Create_WhenTitleIsTooLongOrTooShort_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();
        var title = StringUtilities.GenerateLongString(stringLength);

        // Act
        var act = () => Domain.Book.Book.Create(
            title,
            Constants.Books.Description,
            image,
            user,
            genres);

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
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();
        var title = StringUtilities.GenerateLongWhiteSpace(stringLength);

        // Act
        var act = () => Domain.Book.Book.Create(
            title,
            Constants.Books.Description,
            image,
            user,
            genres);

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
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();

        var description = StringUtilities.GenerateLongString(descriptionLength);

        // Act
        var act = () => Domain.Book.Book.Create(
            Constants.Books.Title,
            description,
            image,
            user,
            genres);

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
        var genres = new List<Genre.Genre>
        {
            GenreFactory.CreateGenre()
        };
        var image = ImageFactory.CreateImage();
        var user = UserFactory.CreateUser();

        var description = StringUtilities.GenerateLongWhiteSpace(descriptionLength);

        // Act
        var act = () => Domain.Book.Book.Create(
            Constants.Books.Title,
            description,
            image,
            user,
            genres);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Description should be present and not be white space");
    }
}
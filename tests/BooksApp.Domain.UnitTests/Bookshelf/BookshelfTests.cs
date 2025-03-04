using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using TestCommon.Books;
using TestCommon.Bookshelves;
using TestCommon.Common.Constants;
using TestCommon.Common.Helpers;
using TestCommon.Users;

namespace BooksApp.Domain.UnitTests.Bookshelf;

public class BookshelfTests
{
    [Fact]
    public void Create_WhenEverythingIsOK_ShouldReturnBookshelf()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = Domain.Bookshelf.Bookshelf.Create(
            user,
            Constants.Bookshelves.Name);

        // Assert
        result.Should().BeOfType<Domain.Bookshelf.Bookshelf>();
        result.Name.Should().Be(Constants.Bookshelves.Name);
    }
    
    [Fact]
    public void AddBook_WhenEverythingIsOK_ShouldAddBook()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        var book = BookFactory.CreateBook();

        // Act
        bookshelf.AddBook(book);

        // Assert
        bookshelf.HasBook(book.Id.Value).Should().BeTrue();
    }
    
    [Fact]
    public void RemoveBook_WhenEverythingIsOK_ShouldRemoveBook()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        var book = BookFactory.CreateBook();
        bookshelf.AddBook(book);

        // Act
        bookshelf.RemoveBook(book.Id.Value);

        // Assert
        bookshelf.HasBook(book.Id.Value).Should().BeFalse();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Bookshelf.Name + 1)]
    public void Create_WhenNameLengthIsWrong_ShouldThrowAnError(int stringLength)
    {
        // Arrange
        var user = UserFactory.CreateUser();

        var name = StringUtilities.GenerateLongString(stringLength);
        
        // Act
        var act = () => Domain.Bookshelf.Bookshelf.Create(
            user,
            name);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(20)]
    [InlineData(MaxPropertyLength.Bookshelf.Name)]
    public void Create_WhenNameIsWhiteSpace_ShouldThrowAnError(int whiteSpaceLength)
    {
        // Arrange
        var user = UserFactory.CreateUser();

        var name = StringUtilities.GenerateLongWhiteSpace(whiteSpaceLength);
        
        // Act
        var act = () => Domain.Bookshelf.Bookshelf.Create(
            user,
            name);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Name could not be empty");
    }
    
    [Fact]
    public void AddBook_WhenThereIsAlreadyABook_ShouldThrowAnError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        var book = BookFactory.CreateBook();
        bookshelf.AddBook(book);

        // Act
        var act = () => bookshelf.AddBook(book);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Bookshelf already have this book");
    }
    
    [Fact]
    public void RemoveBook_WhenThereIsNoBookToRemove_ShouldThrowAnError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        // Act
        var act = () => bookshelf.RemoveBook(Guid.NewGuid());

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Bookshelf does not have this book");
    }
}
using BooksApp.Domain.Book.ValueObjects;
using FluentAssertions;
using TestCommon.Books;
using TestCommon.Bookshelves;
using TestCommon.Common.Constants;
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
}
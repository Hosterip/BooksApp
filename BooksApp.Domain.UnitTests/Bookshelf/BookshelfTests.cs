using FluentAssertions;
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
}
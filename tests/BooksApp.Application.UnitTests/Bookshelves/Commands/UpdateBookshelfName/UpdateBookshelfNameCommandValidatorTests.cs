using BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;
using BooksApp.Application.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.UpdateBookshelfName;

public class UpdateBookshelfNameCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    [Fact]
    public async Task Constructor_WhenEverythingIsOkay_ShouldNotThrowAnError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        
        _unitOfWork.Bookshelves.AnyAsync(x => true).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.GetSingleById(Guid.Empty).ReturnsForAnyArgs(bookshelf);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var updateBookshelfNameCommand = BookshelfCommandFactory
            .CreateUpdateBookshelfNameCommand(bookshelfId: bookshelf.Id.Value);
        var validator = new UpdateBookshelfNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(updateBookshelfNameCommand);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
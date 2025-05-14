using BooksApp.Application.Bookshelves.Commands.AddBook;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.AddBook;

public class AddBookCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookCommand();
        var validator = new AddBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsNoBookshelf_ShouldReturnSpecificError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookCommand();
        var validator = new AddBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().BeGreaterThan(0);
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.Bookshelf.NotFound &&
            x.PropertyName == nameof(AddBookCommand.BookshelfId));
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsNoBook_ShouldReturnSpecificError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(false);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookCommand();
        var validator = new AddBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().BeGreaterThan(0);
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.Book.NotFound &&
            x.PropertyName == nameof(AddBookCommand.BookId));
    }

    [Fact]
    public async Task ValidateAsync_WhenOwnershipUserIsNotAnOwner_ShouldReturnSpecificError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());

        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookCommand();
        var validator = new AddBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().BeGreaterThan(0);
        result.Errors.Should().ContainSingle(x => x.ErrorMessage == ValidationMessages.Bookshelf.NotYours &&
                                                  x.PropertyName == nameof(UserId));
    }
}
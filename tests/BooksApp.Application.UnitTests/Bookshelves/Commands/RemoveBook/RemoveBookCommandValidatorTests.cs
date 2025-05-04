using BooksApp.Application.Bookshelves.Commands.RemoveBook;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.RemoveBook;

public class RemoveBookCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    [Fact]
    public async Task ValidateAsync_PerfectScenario_ShouldReturnValidResult()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        
        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        var command = BookshelfCommandFactory.CreateRemoveBookCommand();
        var validator = new RemoveBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoBook_ShouldReturnInvalidResult()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(false);
        
        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        var command = BookshelfCommandFactory.CreateRemoveBookCommand();
        var validator = new RemoveBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.Book.NotFound)
            .PropertyName
            .Should()
            .Be(nameof(RemoveBookCommand.BookId));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenBookshelfNotFound_ShouldReturnInvalidResult()
    {
        // Arrange
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsNullForAnyArgs();
        _unitOfWork.Bookshelves.AnyBookById(default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.AnyById(default).ReturnsForAnyArgs(false);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());

        var command = BookshelfCommandFactory.CreateRemoveBookCommand();
        var validator = new RemoveBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.NotFound)
            .PropertyName
            .Should()
            .Be(nameof(RemoveBookCommand.BookshelfId));
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.NotYours)
            .PropertyName
            .Should()
            .Be(nameof(UserId));
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.NoBookToRemove)
            .PropertyName
            .Should()
            .Be(nameof(RemoveBookCommand.BookId));
    }
}
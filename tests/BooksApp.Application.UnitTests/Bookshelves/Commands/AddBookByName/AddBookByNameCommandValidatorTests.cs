using BooksApp.Application.Bookshelves.Commands.AddBook;
using BooksApp.Application.Bookshelves.Commands.AddBookByName;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.AddBookByName;

public class AddBookByNameCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.UserId.Value);
        
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookByNameCommand();
        var validator = new AddBookByNameCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoBook_ShouldReturnSpecificError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.UserId.Value);
        
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(false);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(false);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookByNameCommand();
        var validator = new AddBookByNameCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Count.Should().BeGreaterThan(0);
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Book.NotFound)
            .PropertyName
            .Should().Be(nameof(AddBookByNameCommand.BookId));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsAlreadyABook_ShouldReturnSpecificError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.UserId.Value);
        
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(true);
        _unitOfWork.Books.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateAddBookByNameCommand();
        var validator = new AddBookByNameCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Count.Should().BeGreaterThan(0);
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.AlreadyExists)
            .PropertyName
            .Should().Be(nameof(AddBookByNameCommand.BookshelfName));
    }
}
using BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;
using BooksApp.Application.Common.Constants.ValidationMessages;
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
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldBeValid()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        
        _unitOfWork.Bookshelves.AnyAsync(default!).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var updateBookshelfNameCommand = BookshelfCommandFactory
            .CreateUpdateBookshelfNameCommand(bookshelfId: bookshelf.Id.Value);
        var validator = new UpdateBookshelfNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(updateBookshelfNameCommand);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenBookshelfIsNotPossessedByAnUser_ShouldReturnSpecificErrorMessage()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        
        _unitOfWork.Bookshelves.AnyAsync(default!).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var updateBookshelfNameCommand = BookshelfCommandFactory
            .CreateUpdateBookshelfNameCommand(bookshelfId: bookshelf.Id.Value);
        var validator = new UpdateBookshelfNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(updateBookshelfNameCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == ValidationMessages.Bookshelf.NotYours).Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenOldBookshelfAndNewBookshelfNameAreTheSame_ShouldReturnSpecificErrorMessage()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        
        _unitOfWork.Bookshelves.AnyAsync(default!).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var updateBookshelfNameCommand = BookshelfCommandFactory
            .CreateUpdateBookshelfNameCommand(bookshelfId: bookshelf.Id.Value, newName: bookshelf.Name);
        var validator = new UpdateBookshelfNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(updateBookshelfNameCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == ValidationMessages.Bookshelf.NameIsTheSameAsItWas).Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenEveryErrorOccurs_ShouldTwoErrorMessages()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        
        _unitOfWork.Bookshelves.AnyAsync(default!).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.GetSingleById(default).ReturnsForAnyArgs(bookshelf);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var updateBookshelfNameCommand = BookshelfCommandFactory
            .CreateUpdateBookshelfNameCommand(bookshelfId: bookshelf.Id.Value, newName: bookshelf.Name);
        var validator = new UpdateBookshelfNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(updateBookshelfNameCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == ValidationMessages.Bookshelf.NotYours)
            .Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == ValidationMessages.Bookshelf.NameIsTheSameAsItWas)
            .Should().BeTrue();
    }
}
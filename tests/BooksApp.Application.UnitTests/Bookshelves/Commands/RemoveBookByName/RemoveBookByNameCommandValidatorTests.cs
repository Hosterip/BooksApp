using BooksApp.Application.Bookshelves.Commands.RemoveBookByName;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.RemoveBookByName;

public class RemoveBookByNameCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();


    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldBeValid()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(true);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var command = BookshelfCommandFactory.CreateRemoveBookByNameCommand();
        var validator = new RemoveBookByNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoBookToRemove_ShouldBeInvalid()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(false);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var command = BookshelfCommandFactory.CreateRemoveBookByNameCommand();
        var validator = new RemoveBookByNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Should().Contain(x => x.ErrorMessage == ValidationMessages.Bookshelf.NoBookToRemove);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RemoveBookByNameCommand.BookshelfName));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserIsNotFound_ShouldBeInvalid()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(true);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var command = BookshelfCommandFactory.CreateRemoveBookByNameCommand();
        var validator = new RemoveBookByNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Should().Contain(x => x.ErrorMessage == ValidationMessages.User.NotFound);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UserId));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoBookToRemoveNorUserIsFound_ShouldBeInvalid()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(false);
        _unitOfWork.Bookshelves.AnyBookByName(default!, default, default).ReturnsForAnyArgs(false);

        _userService.GetId().ReturnsForAnyArgs(Guid.Empty);

        var command = BookshelfCommandFactory.CreateRemoveBookByNameCommand();
        var validator = new RemoveBookByNameCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Should().Contain(x => x.ErrorMessage == ValidationMessages.User.NotFound);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UserId));
        result.Errors.Should().Contain(x => x.ErrorMessage == ValidationMessages.Bookshelf.NoBookToRemove);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RemoveBookByNameCommand.BookshelfName));
    }
}
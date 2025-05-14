using BooksApp.Application.Bookshelves.Commands.CreateBookshelf;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.CreateBookshelf;

public class CreateBookshelfValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        _unitOfWork.Bookshelves.AnyByName(default!, default).ReturnsForAnyArgs(false);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateCreateBookshelfCommand();
        var validator = new CreateBookshelfValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsAlreadyABookshelf_ShouldReturnSpecificError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        _unitOfWork.Bookshelves.AnyByName(default!, default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateCreateBookshelfCommand();
        var validator = new CreateBookshelfValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.AlreadyHaveWithSameName).PropertyName
            .Should().Be(nameof(CreateBookshelfCommand.Name));
    }
}
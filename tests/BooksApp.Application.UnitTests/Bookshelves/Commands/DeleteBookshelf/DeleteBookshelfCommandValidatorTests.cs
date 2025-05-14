using BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.DeleteBookshelf;

public class DeleteBookshelfCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    [Fact]
    public async Task ValidateAsync_PerfectScenario_ShouldPassValidation()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _unitOfWork.Bookshelves.GetSingleById(bookshelf.Id.Value).Returns(bookshelf);
        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        var command = BookshelfCommandFactory.CreateDeleteBookshelfCommand(bookshelf.Id.Value);
        var validator = new DeleteBookshelfCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    public async Task ValidateAsync_WhenOneDoesNotOwnBookshelf_ShouldHaveAnError()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();

        _unitOfWork.Bookshelves.GetSingleById(bookshelf.Id.Value).Returns(bookshelf);
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());

        var command = BookshelfCommandFactory.CreateDeleteBookshelfCommand(bookshelf.Id.Value);
        var validator = new DeleteBookshelfCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.NotYours).PropertyName
            .Should().Be(nameof(UserId));
    }

    [Theory]
    [InlineData(DefaultBookshelvesNames.Read)]
    [InlineData(DefaultBookshelvesNames.ToRead)]
    [InlineData(DefaultBookshelvesNames.CurrentlyReading)]
    public async Task ValidateAsync_WhenBookshelfIsOneOfDefaultOnes_ShouldHaveAnError(string bookshelfName)
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf(name: bookshelfName);

        _unitOfWork.Bookshelves.GetSingleById(bookshelf.Id.Value).Returns(bookshelf);
        _userService.GetId().ReturnsForAnyArgs(bookshelf.User.Id.Value);

        var command = BookshelfCommandFactory.CreateDeleteBookshelfCommand(bookshelf.Id.Value);
        var validator = new DeleteBookshelfCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.Bookshelf.CannotDeleteDefault).PropertyName
            .Should().Be(nameof(DeleteBookshelfCommand.BookshelfId));
    }
}
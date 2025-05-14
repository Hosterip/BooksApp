using BooksApp.Application.Books.Commands.UpdateBook;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Books;
using TestCommon.Common.Helpers;
using TestCommon.Genres;

namespace BooksApp.Application.UnitTests.Books.Command.UpdateBook;

public class UpdateBookCommandValidatorTests
{
    private readonly IImageFileBuilder _imageFileBuilder = Substitute.For<IImageFileBuilder>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UpdateBookCommandValidatorTests()
    {
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyAsync(default!).ReturnsForAnyArgs(true);
        _unitOfWork.Books.AnyByTitle(default, default!).ReturnsForAnyArgs(false);
        _unitOfWork.Books.AnyAsync(default!).ReturnsForAnyArgs(true);
        _unitOfWork.Genres.GetAllByIds(default!).ReturnsForAnyArgs([GenreFactory.CreateGenre()]);

        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());

        _imageFileBuilder.IsValid(default!).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var command = BookCommandFactory.CreateUpdateBookCommand();
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(MaxPropertyLength.Book.Title)]
    public async Task ValidateAsync_WhenTitleIsWhitespace_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command =
            BookCommandFactory.CreateUpdateBookCommand(title: StringUtilities.GenerateLongWhiteSpace(stringLength));
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should()
            .Contain(x => x.PropertyName == nameof(command.Title));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Title + 1)]
    public async Task ValidateAsync_WhenTitleExceedsMaxLength_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command =
            BookCommandFactory.CreateUpdateBookCommand(title: StringUtilities.GenerateLongString(stringLength));
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should()
            .Contain(x => x.PropertyName == nameof(command.Title));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(MaxPropertyLength.Book.Description)]
    public async Task ValidateAsync_WhenDescriptionIsWhitespace_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command =
            BookCommandFactory.CreateUpdateBookCommand(
                description: StringUtilities.GenerateLongWhiteSpace(stringLength));
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should()
            .Contain(x => x.PropertyName == nameof(command.Description));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Description + 1)]
    public async Task ValidateAsync_WhenDescriptionExceedsMaxLength_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command =
            BookCommandFactory.CreateUpdateBookCommand(description: StringUtilities.GenerateLongString(stringLength));
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should()
            .Contain(x => x.PropertyName == nameof(command.Description));
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsAlreadyABookWithASameTitle_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Books.AnyByTitle(default, default!).ReturnsForAnyArgs(true);

        var command = BookCommandFactory.CreateUpdateBookCommand();
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(x =>
                x.ErrorMessage == ValidationMessages.Book.WithSameNameAlreadyExists &&
                x.PropertyName == nameof(UpdateBookCommand.Title));
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsNoGenres_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Genres.GetAllByIds(default!).ReturnsForAnyArgs([]);

        var command = BookCommandFactory.CreateUpdateBookCommand();
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(x =>
                x.ErrorMessage == ValidationMessages.Book.GenresNotFound &&
                x.PropertyName == nameof(UpdateBookCommand.GenreIds));
    }

    [Fact]
    public async Task ValidateAsync_WhenImageHasInvalidName_ShouldReturnSpecificError()
    {
        // Arrange
        _imageFileBuilder.IsValid(default!).ReturnsForAnyArgs(false);

        var command = BookCommandFactory.CreateUpdateBookCommand();
        var validator = new UpdateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(x =>
                x.ErrorMessage == ValidationMessages.Image.InvalidFileName &&
                x.PropertyName == nameof(UpdateBookCommand.Image));
    }
}
using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Books;
using TestCommon.Common.Helpers;
using TestCommon.Genres;

namespace BooksApp.Application.UnitTests.Books.Command.CreateBook;

public class CreateBookCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IImageFileBuilder _imageFileBuilder = Substitute.For<IImageFileBuilder>();

    public CreateBookCommandValidatorTests()
    {
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyAsync(default!).ReturnsForAnyArgs(true);
        _unitOfWork.Books.AnyByTitle(default, default!).ReturnsForAnyArgs(false);
        _unitOfWork.Genres.GetAllByIds(default!).ReturnsForAnyArgs([GenreFactory.CreateGenre()]);
        
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());

        _imageFileBuilder.IsValid(default!).ReturnsForAnyArgs(true);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldBeValid()
    {
        // Arrange
        var command = BookCommandFactory.CreateCreateBookCommand();
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

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
        var command = BookCommandFactory.CreateCreateBookCommand(title: StringUtilities.GenerateLongWhiteSpace(stringLength));
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.PropertyName == nameof(command.Title)).Should().BeTrue();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Title + 1)]
    public async Task ValidateAsync_WhenTitleExceedsMaxLength_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command = BookCommandFactory.CreateCreateBookCommand(title: StringUtilities.GenerateLongString(stringLength));
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.PropertyName == nameof(command.Title)).Should().BeTrue();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(MaxPropertyLength.Book.Description)]
    public async Task ValidateAsync_WhenDescriptionIsWhitespace_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command = BookCommandFactory.CreateCreateBookCommand(description: StringUtilities.GenerateLongWhiteSpace(stringLength));
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.PropertyName == nameof(command.Description)).Should().BeTrue();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Book.Description + 1)]
    public async Task ValidateAsync_WhenDescriptionExceedsMaxLength_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var command = BookCommandFactory.CreateCreateBookCommand(description: StringUtilities.GenerateLongString(stringLength));
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Any(x => x.PropertyName == nameof(command.Description)).Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserIsNotAnAuthor_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Users.AnyAsync(default!).ReturnsForAnyArgs(false);

        var command = BookCommandFactory.CreateCreateBookCommand();
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Book.MustBeAnAuthor).PropertyName
            .Should().Be(nameof(UserId));
    }
    
    
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsAlreadyABookWithTheSameName_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Books.AnyByTitle(default, default!).ReturnsForAnyArgs(true);

        var command = BookCommandFactory.CreateCreateBookCommand();
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Book.WithSameNameAlreadyExists).PropertyName
            .Should().Be(nameof(CreateBookCommand.Title));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoGenres_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Genres.GetAllByIds(default!).ReturnsForAnyArgs([]);
        
        var command = BookCommandFactory.CreateCreateBookCommand();
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Book.GenresNotFound).PropertyName
            .Should().Be(nameof(CreateBookCommand.GenreIds));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenImageIsNotValid_ShouldReturnSpecificError()
    {
        // Arrange
        _imageFileBuilder.IsValid(default!).ReturnsForAnyArgs(false);

        var command = BookCommandFactory.CreateCreateBookCommand();
        var validator = new CreateBookCommandValidator(_unitOfWork, _imageFileBuilder, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Image.WrongFileName).PropertyName
            .Should().Be("Image.FileName");
    }
}
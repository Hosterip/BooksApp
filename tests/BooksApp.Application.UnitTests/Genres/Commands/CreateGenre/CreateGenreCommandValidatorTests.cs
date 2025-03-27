using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Genres.Commands.CreateGenre;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Common.Helpers;
using TestCommon.Genres;

namespace BooksApp.Application.UnitTests.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    public CreateGenreCommandValidatorTests()
    {
        _unitOfWork.Genres.AnyAsync(default!).ReturnsForAnyArgs(false);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        var command = GenreCommandFactory.CreateCreateGenreCommand();
        var validator = new CreateGenreCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsAlreadyAGenreWithSameName_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Genres.AnyAsync(default!).ReturnsForAnyArgs(true);

        var command = GenreCommandFactory.CreateCreateGenreCommand();
        var validator = new CreateGenreCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
                x.ErrorMessage == ValidationMessages.Genre.AlreadyExists &&
                x.PropertyName == nameof(CreateGenreCommand.Name)
            );
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(MaxPropertyLength.Genre.Name)]
    public async Task ValidateAsync_WhenNameIsPurelyAWhitespace_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var name = StringUtilities.GenerateLongWhiteSpace(stringLength);
        
        var command = GenreCommandFactory.CreateCreateGenreCommand(name: name);
        var validator = new CreateGenreCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(CreateGenreCommand.Name)
        );
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.Genre.Name + 1)]
    public async Task ValidateAsync_WhenNameDoesNotMeetLengthRequirements_ShouldReturnSpecificError(int stringLength)
    {
        // Arrange
        var name = StringUtilities.GenerateLongString(stringLength);
        
        var command = GenreCommandFactory.CreateCreateGenreCommand(name: name);
        var validator = new CreateGenreCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(CreateGenreCommand.Name)
        );
    }
}
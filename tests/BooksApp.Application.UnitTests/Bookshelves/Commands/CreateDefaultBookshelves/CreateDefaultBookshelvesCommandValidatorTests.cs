using BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.CreateDefaultBookshelves;

public class CreateDefaultBookshelvesCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task ValidateAsync_PerfectScenario_ShouldBeValid()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);

        var command = BookshelfCommandFactory.CreateCreateDefaultBookshelvesCommand();
        var validator = new CreateDefaultBookshelvesCommandValidator(_unitOfWork);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserIsNotFound_ShouldHaveAnError()
    {
        // Arrange
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(false);

        var command = BookshelfCommandFactory.CreateCreateDefaultBookshelvesCommand();
        var validator = new CreateDefaultBookshelvesCommandValidator(_unitOfWork);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Single(x => x.ErrorMessage == ValidationMessages.User.NotFound).PropertyName
            .Should().Be(nameof(CreateDefaultBookshelvesCommand.UserId));
    }
}
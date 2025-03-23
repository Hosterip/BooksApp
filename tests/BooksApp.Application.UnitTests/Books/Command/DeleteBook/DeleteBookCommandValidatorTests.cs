using BooksApp.Application.Books.Commands.DeleteBook;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Books;

namespace BooksApp.Application.UnitTests.Books.Command.DeleteBook;

public class DeleteBookCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public DeleteBookCommandValidatorTests()
    {
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _unitOfWork.Books.AnyAsync(default!).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        var command = BookCommandFactory.CreateDeleteBookCommand();
        var validator = new DeleteBookCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task ValidateAsync_WhenUserIsNotAnOwnerOfABook_ShouldReturnASpecificError()
    {
        // Arrange
        _unitOfWork.Books.AnyAsync(default!).ReturnsForAnyArgs(false);
        
        var command = BookCommandFactory.CreateDeleteBookCommand();
        var validator = new DeleteBookCommandValidator(_unitOfWork, _userService);
        
        // Act
        var result = await validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Single(x => x.ErrorMessage == ValidationMessages.Book.BookNotYour).PropertyName
            .Should().Be(nameof(UserId));
    }
}
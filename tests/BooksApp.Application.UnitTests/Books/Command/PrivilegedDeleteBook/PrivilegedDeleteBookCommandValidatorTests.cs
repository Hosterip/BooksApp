using BooksApp.Application.Books.Commands.PrivilegedDeleteBook;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Role;
using BooksApp.Domain.User.ValueObjects;
using FluentAssertions;
using NSubstitute;
using TestCommon.Books;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Books.Command.PrivilegedDeleteBook;

public class PrivilegedDeleteBookCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public PrivilegedDeleteBookCommandValidatorTests()
    {
        var user = UserFactory.CreateUser(RoleFactory.Admin());

        _unitOfWork.Users.GetSingleById(default).ReturnsForAnyArgs(user);
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _unitOfWork.Books.AnyById(default!).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyById(default!).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsGood_ShouldReturnValidResult()
    {
        // Arrange
        var command = BookCommandFactory.CreatePrivilegedDeleteBookCommand();
        var validator = new PrivilegedDeleteBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenThereIsNoSpecifiedBook_ShouldReturnSpecificError()
    {
        // Arrange
        _unitOfWork.Books.AnyById(default!).ReturnsForAnyArgs(false);

        var command = BookCommandFactory.CreatePrivilegedDeleteBookCommand();
        var validator = new PrivilegedDeleteBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.Book.NotFound &&
            x.PropertyName == nameof(PrivilegedDeleteBookCommand.Id));
    }

    [Fact]
    public async Task ValidateAsync_WhenUserHasNotAdminRole_ShouldReturnSpecificError()
    {
        // Arrange
        var user = UserFactory.CreateUser(RoleFactory.Member());

        _unitOfWork.Users.GetSingleById(default).ReturnsForAnyArgs(user);

        _unitOfWork.Books.AnyAsync(default!).ReturnsForAnyArgs(false);

        var command = BookCommandFactory.CreatePrivilegedDeleteBookCommand();
        var validator = new PrivilegedDeleteBookCommandValidator(_unitOfWork, _userService);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.User.Permission &&
            x.PropertyName == nameof(UserId)
        );
    }
}
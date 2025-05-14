using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.UpdateName;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Common.Helpers;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Users.Commands.UpdateName;

public class UpdateNameCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UpdateNameCommandValidatorTests()
    {
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _unitOfWork.Users.AnyById(Guid.NewGuid()).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        var command = UserCommandFactory.CreateUpdateNameCommand();
        var validator = new UpdateNameCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.User.FirstName + 1)]
    public async Task ValidateAsync_WhenFirstNameExceedsProperLength_ShouldReturnSpecificError(int firstNameLength)
    {
        // Arrange
        var firstName = StringUtilities.GenerateLongString(firstNameLength);
        var command = UserCommandFactory.CreateUpdateNameCommand(firstName);
        var validator = new UpdateNameCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateNameCommand.FirstName));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.User.FirstName)]
    public async Task ValidateAsync_WhenFirstNameIsPureWhitespace_ShouldReturnSpecificError(int firstNameLength)
    {
        // Arrange
        var firstName = StringUtilities.GenerateLongWhiteSpace(firstNameLength);
        var command = UserCommandFactory.CreateUpdateNameCommand(firstName);
        var validator = new UpdateNameCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateNameCommand.FirstName));
    }

    [Fact]
    public async Task ValidateAsync_WhenMiddleNameExceedsProperLength_ShouldReturnSpecificError()
    {
        // Arrange
        var middleName = StringUtilities.GenerateLongWhiteSpace(MaxPropertyLength.User.MiddleName + 1);
        var command = UserCommandFactory.CreateUpdateNameCommand(middleName: middleName);
        var validator = new UpdateNameCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(UpdateNameCommand.MiddleName));
    }

    [Fact]
    public async Task ValidateAsync_WhenLastNameExceedsProperLength_ShouldReturnSpecificError()
    {
        // Arrange
        var lastName = StringUtilities.GenerateLongWhiteSpace(MaxPropertyLength.User.LastName + 1);
        var command = UserCommandFactory.CreateUpdateNameCommand(lastName: lastName);
        var validator = new UpdateNameCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == nameof(UpdateNameCommand.LastName));
    }
}
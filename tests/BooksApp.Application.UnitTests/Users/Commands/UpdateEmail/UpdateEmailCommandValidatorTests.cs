using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Common.Helpers;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Users.Commands.UpdateEmail;

public class UpdateEmailCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UpdateEmailCommandValidatorTests()
    {
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _unitOfWork.Users.AnyByEmail(default!).ReturnsForAnyArgs(false);
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        var command = UserCommandFactory.CreateUpdateEmailCommand();
        var validator = new UpdateEmailCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert 
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenEmailIsOccupied_ShouldReturnASpecificError()
    {
        // Arrange
        _unitOfWork.Users.AnyByEmail(default!).ReturnsForAnyArgs(true);

        var command = UserCommandFactory.CreateUpdateEmailCommand();
        var validator = new UpdateEmailCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert 
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.User.Occupied &&
            x.PropertyName == nameof(UpdateEmailCommand.Email));
    }

    [Theory]
    [InlineData("@")]
    [InlineData("lo@2.")]
    [InlineData("gmail.com")]
    [InlineData(".")]
    public async Task ValidateAsync_WhenEmailIsInappropriate_ShouldReturnASpecificError(string email)
    {
        // Arrange
        var command = UserCommandFactory.CreateUpdateEmailCommand(email: email);
        var validator = new UpdateEmailCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert 
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x =>
            x.ErrorMessage == ValidationMessages.User.InappropriateEmail &&
            x.PropertyName == nameof(UpdateEmailCommand.Email));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.User.Email + 1)]
    public async Task ValidateAsync_WhenEmailHasInappropriateLength_ShouldReturnASpecificError(int emailLength)
    {
        // Arrange
        var email = StringUtilities.GenerateLongString(emailLength);
        var command = UserCommandFactory.CreateUpdateEmailCommand(email: email);
        var validator = new UpdateEmailCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert 
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateEmailCommand.Email));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.User.Email)]
    public async Task ValidateAsync_WhenEmailIsFullOfWhitespace_ShouldReturnASpecificError(int emailLength)
    {
        // Arrange
        var email = StringUtilities.GenerateLongWhiteSpace(emailLength);
        var command = UserCommandFactory.CreateUpdateEmailCommand(email: email);
        var validator = new UpdateEmailCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert 
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateEmailCommand.Email));
    }
}
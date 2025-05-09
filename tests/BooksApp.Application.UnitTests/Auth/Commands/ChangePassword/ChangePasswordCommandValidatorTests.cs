using BooksApp.Application.Auth.Commands.ChangePassword;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using TestCommon.Auth;
using TestCommon.Common.Helpers;

namespace BooksApp.Application.UnitTests.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidatorTests
{
    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnValidResult()
    {
        // Arrange
        //  Creating command
        var command = AuthCommandFactory.CreateChangePasswordCommand();
        
        //  Creating validator
        var validator = new ChangePasswordCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.User.Password + 1)]
    public async Task ValidateAsync_WhenPasswordExceedsMaxLength_ShouldReturnInvalidResult(int passwordLength)
    {
        // Arrange
        //  Creating password
        var password = StringUtilities.GenerateLongString(passwordLength);
        //  Creating command
        var command = AuthCommandFactory.CreateChangePasswordCommand(newPassword:password);
        
        //  Creating validator
        var validator = new ChangePasswordCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == command.NewPassword);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.User.Password)]
    public async Task ValidateAsync_WhenPasswordIsWhitespace_ShouldReturnInvalidResult(int passwordLength)
    {
        // Arrange
        //  Creating password
        var password = StringUtilities.GenerateLongWhiteSpace(passwordLength);
        
        //  Creating command
        var command = AuthCommandFactory.CreateChangePasswordCommand(password);
        
        //  Creating validator
        var validator = new ChangePasswordCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == command.NewPassword);
    }
}
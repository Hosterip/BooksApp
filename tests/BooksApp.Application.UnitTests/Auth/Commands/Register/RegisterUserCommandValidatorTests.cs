using BooksApp.Application.Auth.Commands.Register;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Auth;
using TestCommon.Common.Helpers;

namespace BooksApp.Application.UnitTests.Auth.Commands.Register;

public class RegisterUserCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    public RegisterUserCommandValidatorTests()
    {
        _unitOfWork.Users.AnyByEmail(default).ReturnsForAnyArgs(false);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        //  Creating a command
        var command = AuthCommandFactory.CreateRegisterUserCommand();

        //  Creating a validator
        var validator = new RegisterUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("@com.com")]
    [InlineData("foo@@foo.com")]
    [InlineData("foo.com")]
    public async Task ValidateAsync_WhenEmailIsInvalid_ShouldReturnASpecificError(string email)
    {
        // Arrange
        //  Creating a command
        var command = AuthCommandFactory.CreateRegisterUserCommand(email: email);

        //  Creating a validator
        var validator = new RegisterUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.ErrorMessage == ValidationMessages.User.InappropriateEmail &&
            x.PropertyName == nameof(command.Email));
    }
    
    [Fact]
    public async Task ValidateAsync_WhenEmailIsOccupied_ShouldReturnASpecificError()
    {
        // Arrange
        //  Making AnyByEmail method return true
        _unitOfWork.Users.AnyByEmail(default).ReturnsForAnyArgs(true);

        //  Creating a command
        var command = AuthCommandFactory.CreateRegisterUserCommand();

        //  Creating a validator
        var validator = new RegisterUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.ErrorMessage == ValidationMessages.Auth.Occupied &&
            x.PropertyName == nameof(command.Email));
    }   
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.User.FirstName + 1)]
    public async Task ValidateAsync_WhenFirstNameExceedsProperLength_ShouldReturnSpecificError(int firstNameLength)
    {
        // Arrange
        //  Creating inflated string
        var firstName = StringUtilities.GenerateLongString(firstNameLength);
        
        //  Creating a command
        var command = AuthCommandFactory.CreateRegisterUserCommand(firstName: firstName);

        //  Creating a validator
        var validator = new RegisterUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => 
            x.PropertyName == nameof(command.FirstName));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.User.FirstName)]
    public async Task ValidateAsync_WhenFirstNameIsPureWhitespace_ShouldReturnSpecificError(int firstNameLength)
    {
        // Arrange
        //  Creating inflated string
        var firstName = StringUtilities.GenerateLongWhiteSpace(firstNameLength);
        
        //  Creating a command
        var command = AuthCommandFactory.CreateRegisterUserCommand(firstName: firstName);

        //  Creating a validator
        var validator = new RegisterUserCommandValidator(_unitOfWork);

        // Act
        var result = await validator.ValidateAsync(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.PropertyName == nameof(command.FirstName));
    }
}
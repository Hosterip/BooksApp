using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.InsertAvatar;
using FluentAssertions;
using NSubstitute;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Users.Commands.InsertAvatar;

public class InsertAvatarCommandValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IImageFileBuilder _imageFileBuilder = Substitute.For<IImageFileBuilder>();

    public InsertAvatarCommandValidatorTests()
    {
        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _unitOfWork.Users.AnyById(default).ReturnsForAnyArgs(true);
        _imageFileBuilder.IsValid(default!).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        var command = UserCommandFactory.CreateInsertAvatarCommand();
        var validator = new InsertAvatarCommandValidator(_imageFileBuilder);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenImageIsNotValid_ShouldReturnAValidResult()
    {
        // Arrange
        _imageFileBuilder.IsValid(default!).ReturnsForAnyArgs(false);
        
        var command = UserCommandFactory.CreateInsertAvatarCommand();
        var validator = new InsertAvatarCommandValidator(_imageFileBuilder);
        
        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => 
            x.ErrorMessage == ValidationMessages.Image.InvalidFileName &&
            x.PropertyName == nameof(InsertAvatarCommand.Image));
    }
}
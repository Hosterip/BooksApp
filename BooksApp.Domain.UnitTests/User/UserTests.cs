using BooksApp.Domain.Common;
using BooksApp.Domain.Role;
using FluentAssertions;
using TestCommon.Common.Constants;
using TestCommon.Common.Factories;
using TestCommon.Images;

namespace BooksApp.Domain.UnitTests.User;

public class UserTests
{
    [Fact]
    public void Create_WhenEverythingInOrder_ShouldCreateUser()
    {
        // Arrange
        var image = ImageFactory.CreateImage();
        var role = RoleFactory.Member();
        var passwordHasher = PasswordHasherFactory.CreatePasswordHasher();
        
        // Act
        var result = Domain.User.User.Create(
            passwordHasher,
            Constants.Users.Email,
            role,
            Constants.Users.Password,
            image,
            Constants.Users.FirstName);

        // Assert
        result.Should().BeOfType<Domain.User.User>();
        result.FirstName.Should().Be(Constants.Users.FirstName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("", "", "")]
    public void Create_WhenNameIsNotRight_ShouldThrowAnError(
        string firstName,
        string? middleName = null,
        string? lastName = null)
    {
        // Arrange
        var image = ImageFactory.CreateImage();
        var role = RoleFactory.Member();
        var passwordHasher = PasswordHasherFactory.CreatePasswordHasher();
        
        // Act
        var act = () => Domain.User.User.Create(
            passwordHasher,
            Constants.Users.Email,
            role,
            Constants.Users.Password,
            image,
            firstName,
            middleName,
            lastName);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("@")]
    [InlineData(".")]
    [InlineData("foo@bar.")]
    public void Create_WhenEmailIsNotRight_ShouldThrowAnError(string email)
    {
        // Arrange
        var image = ImageFactory.CreateImage();
        var role = RoleFactory.Member();
        var passwordHasher = PasswordHasherFactory.CreatePasswordHasher();
        
        // Act
        var act = () => Domain.User.User.Create(
            passwordHasher,
            email,
            role,
            Constants.Users.Password,
            image,
            Constants.Users.FirstName);

        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
}
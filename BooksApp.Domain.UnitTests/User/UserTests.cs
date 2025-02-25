using BooksApp.Domain.Common;
using BooksApp.Domain.Role;
using FluentAssertions;
using TestCommon.Common.Constants;
using TestCommon.Common.Factories;
using TestCommon.Images;
using TestCommon.Users;

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
    
    [Fact]
    public void AddFollower_WhenEverythingInOrder_ShouldAddAFollower()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var follower = UserFactory.CreateUser();

        // Act
        user.AddFollower(follower);
        
        // Assert
        user.HasFollower(follower.Id).Should().BeTrue();
    }
    
    [Fact]
    public void AddFollower_WhenFollowerAlreadyInAList_ShouldThrowAnError()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var follower = UserFactory.CreateUser();
        user.AddFollower(follower);

        // Act
        var act = () => user.AddFollower(follower);
        
        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
    
    [Fact]
    public void RemoveFollower_WhenEverythingInOrder_ShouldRemoveAFollower()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var follower = UserFactory.CreateUser();
        user.AddFollower(follower);

        // Act
        user.RemoveFollower(follower);
        
        // Assert
        user.HasFollower(follower.Id).Should().BeFalse();
    }
    
    [Fact]
    public void RemoveFollower_WhenThereIsNoFollower_ShouldThrowAnError()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var follower = UserFactory.CreateUser();

        // Act
        var act = () => user.RemoveFollower(follower);
        
        // Assert
        Assert.ThrowsAny<DomainException>(act);
    }
}
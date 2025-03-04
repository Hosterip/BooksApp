using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Role;
using FluentAssertions;
using TestCommon.Common.Constants;
using TestCommon.Common.Factories;
using TestCommon.Common.Helpers;
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
    [InlineData(0)]
    [InlineData(0, 0, 0)]
    [InlineData(MaxPropertyLength.User.FirstName + 1)]
    [InlineData(MaxPropertyLength.User.FirstName + 1, MaxPropertyLength.User.MiddleName + 1, MaxPropertyLength.User.LastName + 1)]
    public void Create_WhenNameIsNotRight_ShouldThrowAnError(
        int firstNameLength,
        int? middleNameLength = null,
        int? lastNameLength = null)
    {
        // Arrange
        var image = ImageFactory.CreateImage();
        var role = RoleFactory.Member();
        var passwordHasher = PasswordHasherFactory.CreatePasswordHasher();

        var firstName = StringUtilities.GenerateLongString(firstNameLength);
        var middleName = middleNameLength != null
            ? StringUtilities.GenerateLongString(middleNameLength.Value)
            : null;
        var lastName = lastNameLength != null 
            ? StringUtilities.GenerateLongString(lastNameLength.Value) 
            : null;
        
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
    [InlineData(1, 1, 1)]
    [InlineData(MaxPropertyLength.User.FirstName, MaxPropertyLength.User.MiddleName, MaxPropertyLength.User.LastName)]
    public void Create_WhenNameIsWhiteSpace_ShouldThrowAnError(
        int firstNameLength,
        int middleNameLength,
        int lastNameLength)
    {
        // Arrange
        var image = ImageFactory.CreateImage();
        var role = RoleFactory.Member();
        var passwordHasher = PasswordHasherFactory.CreatePasswordHasher();

        var firstName = StringUtilities.GenerateLongWhiteSpace(firstNameLength);
        var middleName = StringUtilities.GenerateLongWhiteSpace(middleNameLength);
        var lastName = StringUtilities.GenerateLongWhiteSpace(lastNameLength);
        
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
    
    [Fact]
    public void ViewerRelationship_WhenThereIsNoFollower_ShouldReturnATuple()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var secondUser = UserFactory.CreateUser();

        // Act
        var result = user.ViewerRelationship(secondUser.Id.Value);
        
        // Assert
        result.IsFollowing.Should().BeFalse();
        result.IsFriend.Should().BeFalse();
        result.IsMe.Should().BeFalse();
    }
    
    [Fact]
    public void ViewerRelationship_WhenThereIsFollower_ShouldReturnATuple()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var follower = UserFactory.CreateUser();
        user.AddFollower(follower);
        
        // Act
        var result = user.ViewerRelationship(follower.Id.Value);
        
        // Assert
        result.IsFollowing.Should().BeTrue();
        result.IsFriend.Should().BeFalse();
        result.IsMe.Should().BeFalse();
    }
    
    [Fact]
    public void ViewerRelationship_WhenComparingToOneself_ShouldReturnATuple()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        
        // Act
        var result = user.ViewerRelationship(user.Id.Value);
        
        // Assert
        result.IsFollowing.Should().BeFalse();
        result.IsFriend.Should().BeFalse();
        result.IsMe.Should().BeTrue();
    }
}
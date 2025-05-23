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
        // Act
        var result = UserFactory.CreateUser();

        // Assert
        result.Should().BeOfType<Domain.User.User>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0, 0, 0)]
    [InlineData(MaxPropertyLength.User.FirstName + 1)]
    [InlineData(MaxPropertyLength.User.FirstName + 1, MaxPropertyLength.User.MiddleName + 1,
        MaxPropertyLength.User.LastName + 1)]
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
        var act = () =>  UserFactory.CreateUser(firstName: firstName, middleName: middleName, lastName: lastName);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage($"First name length should be between 1 and {MaxPropertyLength.User.FirstName}");
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
        var act = () =>  UserFactory.CreateUser(firstName: firstName, middleName: middleName, lastName: lastName);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("First name should be present");
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
        var act = () => UserFactory.CreateUser(email: email); 
        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Invalid email");
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
        act.Should()
            .Throw<DomainException>()
            .WithMessage("There is already a follower");
    }

    [Fact]
    public void AddFollower_WhenAddingOneself_ShouldThrowAnError()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var act = () => user.AddFollower(user);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Can't add yourself to followers");
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
        act.Should()
            .Throw<DomainException>()
            .WithMessage("No follower to remove");
    }

    [Fact]
    public void RemoveFollower_WhenRemovingOneself_ShouldThrowAnError()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var act = () => user.RemoveFollower(user);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .WithMessage("Can't remove yourself to followers");
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
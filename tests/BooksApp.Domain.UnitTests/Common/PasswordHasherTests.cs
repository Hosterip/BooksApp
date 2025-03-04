using BooksApp.Domain.Common.Interfaces;
using BooksApp.Infrastructure.Authentication;
using FluentAssertions;

namespace BooksApp.Domain.UnitTests.Common;

public class PasswordHasherTests
{
    private readonly IPasswordHasher _passwordHasher = new PasswordHasher();

    [Fact]
    public void IsPasswordValid_WhenHashAndSaltIsRight_ShouldReturnTrue()
    {
        // Arrange
        var password = Guid
            .NewGuid()
            .ToString();
        var hashSalt = _passwordHasher.GenerateHashSalt(password);

        // Act
        var result = _passwordHasher.IsPasswordValid(hashSalt.Hash, hashSalt.Salt, password);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void IsPasswordValid_WhenHashAndSaltInvalid_ShouldReturnFalse()
    {
        // Arrange
        var password = string.Empty;
        var hashSalt = (Hash: string.Empty, Salt: string.Empty);

        // Act
        var result = _passwordHasher.IsPasswordValid(hashSalt.Hash, hashSalt.Salt, password);

        // Assert
        result.Should().BeFalse();
    }
}
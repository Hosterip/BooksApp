using Application.UnitTest.Auth.TestUtils;
using Application.UnitTest.Books.Commands.CreateBookCommandTests.TestUtils;
using FluentAssertions;
using Moq;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace Application.UnitTest.Auth.Commands.ChangePasswordCommandTests;

public class ChangePasswordCommandValidatorTests
{
    private readonly ChangePasswordCommandValidator _validator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public ChangePasswordCommandValidatorTests()
    {
        _mockUnitOfWork = new();
        _validator = new ChangePasswordCommandValidator(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WhenRulesNotMatch_ReturnFailureResult()
    {
        // Arrange
        AuthTestUtils.SetupUsersAnyAsyncMethod(_mockUnitOfWork, false);
        var command = AuthCommandsUtils.ChangePasswordCommand;
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public async Task Handle_WhenRulesMatched_ReturnFailureResult()
    {
        // Arrange
        AuthTestUtils.SetupUsersAnyAsyncMethod(_mockUnitOfWork, true);
        var command = AuthCommandsUtils.ChangePasswordCommand;
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
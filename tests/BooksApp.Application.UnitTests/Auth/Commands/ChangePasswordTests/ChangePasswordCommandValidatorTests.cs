using Application.UnitTest.Auth.Commands.TestUtils;
using Application.UnitTest.Auth.TestUtils;
using FluentAssertions;
using Moq;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Common.Interfaces;

namespace Application.UnitTest.Auth.Commands.ChangePasswordTests;

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
        //   In this case the correct or incorrect command doesn't change anything
        var command = AuthCommandsUtils.ChangePasswordCommandCorrect;
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
        //  In this case the correct or incorrect command doesn't change anything
        var command = AuthCommandsUtils.ChangePasswordCommandCorrect;
        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
using System.Linq.Expressions;
using Application.UnitTest.TestUtils.MockData;
using FluentAssertions;
using Moq;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Roles.Commands.UpdateRole;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Models;

namespace Application.UnitTest.Roles.Commands.UpdateRoleTests;

public class UpdateRoleCommandValidatorTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateRoleCommandValidator _validator;

    public UpdateRoleCommandValidatorTests()
    {
        _unitOfWorkMock = new();
        _validator = new UpdateRoleCommandValidator(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Constructor_ReturnFailureResult_WhenChangerIdMatchesTargetId()
    {
        // Arrange
        ArrangeAllMethodsForValidator(true, true, MockUser.GetUser(RoleConstants.Admin));
        var command = new UpdateRoleCommand { UserId = 1, ChangerId = 1, Role = RoleConstants.Admin};

        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public async Task Constructor_WhenRoleRuleNotSatisfied_ReturnFailureResult()
    {
        // Arrange
        ArrangeAllMethodsForValidator(true, true, MockUser.GetUser(RoleConstants.Member));
        var command = new UpdateRoleCommand { UserId = 1, ChangerId = 2, Role = RoleConstants.Admin };

        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public async Task Constructor_WhenRoleRuleSatisfied_ReturnSuccessfulResult()
    {
        // Arrange
        ArrangeAllMethodsForValidator(true, true, MockUser.GetUser(RoleConstants.Admin));
        var command = new UpdateRoleCommand { UserId = 1, ChangerId = 2, Role = RoleConstants.Admin };

        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task Constructor_WhenAllTheRulesSatisfied_ReturnSuccessfulResult()
    {
        // Arrange
        ArrangeAllMethodsForValidator(true, true, MockUser.GetUser(RoleConstants.Admin));
        var command = new UpdateRoleCommand { UserId = 1, ChangerId = 2, Role = RoleConstants.Admin};

        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }

    private void ArrangeAllMethodsForValidator(
        bool roleAnyAsync, 
        bool usersAnyAsync,
        User? usersGetSingle)
    {
        _unitOfWorkMock.Setup(x => x.Roles.AnyAsync(
                It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(roleAnyAsync);
        _unitOfWorkMock.Setup(x => x.Users.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(usersAnyAsync);
        _unitOfWorkMock.Setup(x => x.Users.GetSingleWhereAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(usersGetSingle);
    }
    
}
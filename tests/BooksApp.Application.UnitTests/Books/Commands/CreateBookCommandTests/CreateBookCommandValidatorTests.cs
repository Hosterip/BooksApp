using System.Linq.Expressions;
using Application.UnitTest.Books.Commands.CreateBookCommandTests.TestUtils;
using Application.UnitTest.TestUtils.MockData;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;
using PostsApp.Domain.User;

namespace Application.UnitTest.Books.Commands.CreateBookCommandTests;

public class CreateBookCommandValidatorTests
{
    private readonly CreateBookCommandValidator _validator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public CreateBookCommandValidatorTests()
    {
        _unitOfWorkMock = new();
        _validator = new CreateBookCommandValidator(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Constructor_Should_ReturnFailedResult_WhenEveryRuleWithUserIsNotMatched()
    {
        // Arrange
        var command = CreateBookCommandUtils.CreateBookCommandMethod();
        ArrangeAllMethodsForValidator(false, null);
        // Act
        var result = await _validator.ValidateAsync(command);
        // Assert

        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public async Task Constructor_Should_ReturnSuccessfulResult_WhenEveryRuleMatch()
    {
        // Arrange
        var command = CreateBookCommandUtils.CreateBookCommandMethod();
        ArrangeAllMethodsForValidator(true, MockUser.GetUser(RoleNames.Author));
        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    private void ArrangeAllMethodsForValidator(
        bool usersAnyAsync,
        User? usersGetSingle)
    {
        _unitOfWorkMock.Setup(x => x.Users.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(usersAnyAsync);
        _unitOfWorkMock.Setup(x => x.Users.GetSingleWhereAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(usersGetSingle);
    } 
}
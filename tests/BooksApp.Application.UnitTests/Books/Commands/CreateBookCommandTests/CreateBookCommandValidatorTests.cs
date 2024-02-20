using System.Linq.Expressions;
using Application.UnitTest.MockData;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

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
        var command = new CreateBookCommand{UserId = 1, Title = "1", Description = "1"};
        _unitOfWorkMock.Setup(x => x.Users.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(false);
        // Act
        var result = await _validator.ValidateAsync(command);
        // Assert

        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public async Task Constructor_Should_ReturnFailedResult_WhenEveryRuleMatch()
    {
        // Arrange
        var command = new CreateBookCommand{UserId = 1, Title = "1", Description = "1"};
        _unitOfWorkMock.Setup(x => x.Users.AnyAsync(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(true);
        // Act
        var result = await _validator.ValidateAsync(command);
        // Assert

        result.IsValid.Should().BeTrue();
    }
}
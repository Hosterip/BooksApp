using Application.UnitTest.Auth.TestUtils;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using PostsApp.Application.Auth;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Common.Interfaces;

namespace Application.UnitTest.Auth.Queries.LoginTests;

public class LoginQueryValidatorTests
{
    private readonly LoginUserQueryValidator _validator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public LoginQueryValidatorTests()
    {
        _unitOfWorkMock = new();
        _validator = new LoginUserQueryValidator(_unitOfWorkMock.Object);
    }
    
    [Fact]
    public async Task Handle_CorrectPassword_ReturnAuthResult()
    {
        // Arrange
        // Query with CORRECT password
        var query = AuthQueriesUtils.LoginUserQueryCorrect;
        AuthTestUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var result = await _validator.ValidateAsync(query);
        
        // Assert
        result.Should().BeOfType<ValidationResult>();
        result.IsValid.Should().BeTrue();

    }
    
    [Fact]
    public async Task Constructor_IncorrectPassword_ThrowAnException()
    {
        // Arrange
        // Query with INCORRECT password
        var query = AuthQueriesUtils.LoginUserQueryIncorrect;
        AuthTestUtils.SetupUsersGetSingleWhereAsync(_unitOfWorkMock);
        
        // Act
        var result = await _validator.ValidateAsync(query);
        
        // Assert
        result.Should().BeOfType<ValidationResult>();
        result.IsValid.Should().BeFalse();
    }
}
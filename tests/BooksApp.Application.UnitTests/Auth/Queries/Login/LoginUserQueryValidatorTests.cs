using BooksApp.Application.Auth.Queries.Login;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentAssertions;
using NSubstitute;
using TestCommon.Auth;
using TestCommon.Common.Helpers;

namespace BooksApp.Application.UnitTests.Auth.Queries.Login;

public class LoginUserQueryValidatorTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    public LoginUserQueryValidatorTests()
    {
        _unitOfWork.Users.AnyByEmail(default).ReturnsForAnyArgs(true);
    }

    [Fact]
    public async Task ValidateAsync_WhenEverythingIsOkay_ShouldReturnAValidResult()
    {
        // Arrange
        //  Creating a query
        var query = AuthQueryFactory.CreateLoginUserQuery();
        
        //  Creating a validator
        var validator = new LoginUserQueryValidator(_unitOfWork);
        
        // Act
        var result = await validator.ValidateAsync(query);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ValidateAsync_WhenThereIsNoSuchUser_ShouldReturnAnInvalidResult()
    {
        // Arrange
        //  Making AnyByEmail return false
        _unitOfWork.Users.AnyByEmail(default).ReturnsForAnyArgs(false);
        
        //  Creating a query
        var query = AuthQueryFactory.CreateLoginUserQuery();
        
        //  Creating a validator
        var validator = new LoginUserQueryValidator(_unitOfWork);
        
        // Act
        var result = await validator.ValidateAsync(query);
        
        // Assert
        result.Errors.Should().ContainSingle(x => 
            x.PropertyName == nameof(LoginUserQuery.Email) &&
            x.ErrorMessage == ValidationMessages.User.NotFound);
    }
    
    
    [Theory]
    [InlineData(0)]
    [InlineData(MaxPropertyLength.User.Email + 1)]
    public async Task ValidateAsync_WhenFirstNameExceedsProperLength_ShouldReturnSpecificError(int emailLength)
    {
        // Arrange
        //  Creating inflated string
        var email = StringUtilities.GenerateLongString(emailLength);
        
        //  Creating a query
        var query = AuthQueryFactory.CreateLoginUserQuery(email: email);
        
        //  Creating a validator
        var validator = new LoginUserQueryValidator(_unitOfWork);
        
        // Act
        var result = await validator.ValidateAsync(query);
        
        // Assert
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(LoginUserQuery.Email));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(MaxPropertyLength.User.Password)]
    public async Task ValidateAsync_WhenFirstNameIsPureWhitespace_ShouldReturnSpecificError(int passwordLength)
    {
        // Arrange
        //  Creating inflated string
        var password = StringUtilities.GenerateLongWhiteSpace(passwordLength);
        
        //  Creating a query
        var query = AuthQueryFactory.CreateLoginUserQuery(password: password);
        
        //  Creating a validator
        var validator = new LoginUserQueryValidator(_unitOfWork);
        
        // Act
        var result = await validator.ValidateAsync(query);
        
        // Assert
        result.Errors.Should().ContainSingle(x =>
            x.PropertyName == nameof(LoginUserQuery.Password));
    }
}
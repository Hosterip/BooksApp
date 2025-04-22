using BooksApp.Application.Common.Behaviors;
using BooksApp.Application.Common.Errors;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Queries.GetSingleUser;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Role;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Common.Behaviors;

public class UserValidationBehaviorTests
{
    private static readonly InsertAvatarCommand RequestWithAuthorizeAttribute =
        UserCommandFactory.CreateInsertAvatarCommand();

    private static readonly GetSingleUserQuery RequestWithoutAuthorizeAttribute = new GetSingleUserQuery
    {
        Id = default
    };

    private static RequestHandlerDelegate<UserResult> _next =
        Substitute.For<RequestHandlerDelegate<UserResult>>();

    private IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private IUserService _userService = Substitute.For<IUserService>();

    public UserValidationBehaviorTests()
    {
        var role = RoleFactory.Admin();
        var user = UserFactory.CreateUser(role: role);
        var userResult = UserResultFactory.CreateUserResult();

        _userService.GetId().ReturnsForAnyArgs(Guid.NewGuid());
        _userService.GetSecurityStamp().ReturnsForAnyArgs(user.SecurityStamp);
        _userService.GetRole().ReturnsForAnyArgs(role.Name);

        _unitOfWork.Users.GetSingleById(default).ReturnsForAnyArgs(user);

        _next().ReturnsForAnyArgs(userResult);
    }

    [Fact]
    public async Task Handle_WhenEverythingIsOkayWithAnAttribute_ShouldCallNext()
    {
        // Arrange
        //  Creating behavior  
        var behavior =
            new UserValidationBehavior<InsertAvatarCommand, UserResult>(_userService, _unitOfWork);

        // Act
        var result = await behavior.Handle(RequestWithAuthorizeAttribute, _next, default);

        // Assert 
        result.Should().BeOfType<UserResult>();
    }

    [Fact]
    public async Task Handle_WhenEverythingIsOkayWithoutAnAttribute_ShouldCallNext()
    {
        // Arrange
        //  Creating behavior  
        var behavior = new UserValidationBehavior<GetSingleUserQuery, UserResult>(_userService, _unitOfWork);

        // Act
        var result = await behavior.Handle(RequestWithoutAuthorizeAttribute, _next, default);

        // Assert 
        result.Should().BeOfType<UserResult>();
    }

    [Fact]
    public async Task Handle_WhenSecurityStampIsInvalid_ShouldThrowASpecificError()
    {
        // Arrange
        _userService.GetSecurityStamp().ReturnsForAnyArgs(Guid.NewGuid().ToString());
        //  Creating behavior  
        var behavior =
            new UserValidationBehavior<InsertAvatarCommand, UserResult>(_userService, _unitOfWork);

        // Act
        var act = async () => await behavior.Handle(RequestWithAuthorizeAttribute, _next, default);

        // Assert 
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async Task Handle_WhenUserWasNotFound_ShouldThrowASpecificError()
    {
        // Arrange
        //  Making UserRepository GetSingleById return null
        _unitOfWork.Users.GetSingleById(default).ReturnsNullForAnyArgs();
        
        //  Creating behavior  
        var behavior =
            new UserValidationBehavior<InsertAvatarCommand, UserResult>(_userService, _unitOfWork);

        // Act
        var act = async () => await behavior.Handle(RequestWithAuthorizeAttribute, _next, default);

        // Assert 
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async Task Handle_WhenDbUsersRoleDoesNotCorrespondTheRoleInMemory_ShouldCallNext()
    {
        // Arrange
        _userService.GetRole().ReturnsForAnyArgs(Guid.NewGuid().ToString());
        //  Creating behavior  
        var behavior =
            new UserValidationBehavior<InsertAvatarCommand, UserResult>(_userService, _unitOfWork);

        // Act
        var result = await behavior.Handle(RequestWithAuthorizeAttribute, _next, default);

        // Assert 
        result.Should().BeOfType<UserResult>();
    }
}
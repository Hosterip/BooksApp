using BooksApp.Application.Common.Behaviors;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Role;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using TestCommon.Users;

namespace BooksApp.Application.UnitTests.Common.Behaviors;

public class UserValidationBehaviorTests
{
    private static readonly InsertAvatarCommand _commandWithAuthorizeAttribute = UserCommandFactory.CreateInsertAvatarCommand();
    private static RequestHandlerDelegate<UserResult> _next = Substitute.For<RequestHandlerDelegate<UserResult>>();

    private IHttpContextAccessor _accessor = Substitute.For<IHttpContextAccessor>();
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
    public async Task Handle_WhenEverythingIsOkay_ShouldCallNext()
    {
        // Arrange
        //  Creating behavior  
        var behavior = new UserValidationBehavior<InsertAvatarCommand, UserResult>(_accessor, _userService, _unitOfWork);
        
        // Act
        var result = await behavior.Handle(_commandWithAuthorizeAttribute, _next, default);
        
        // Assert 
        result.Should().BeOfType<UserResult>(); 
    }
}
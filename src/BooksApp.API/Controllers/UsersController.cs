using BooksApp.API.Common.Constants;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Roles.Commands.UpdateRole;
using BooksApp.Application.Users.Commands.AddRemoveFollower;
using BooksApp.Application.Users.Commands.DeleteUser;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Application.Users.Commands.UpdateName;
using BooksApp.Application.Users.Queries.GetSingleUser;
using BooksApp.Application.Users.Queries.GetUserRelationships;
using BooksApp.Application.Users.Queries.GetUsers;
using BooksApp.Contracts.Errors;
using BooksApp.Contracts.Roles;
using BooksApp.Contracts.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class UsersController(
    ISender sender,
    IOutputCacheStore outputCacheStore,
    IMapper mapster,
    IUserService userService)
    : ApiController
{
    #region Users endpoints

    #region Get endpoints
    
    [HttpGet(ApiRoutes.Users.GetMe)]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetMe(
        CancellationToken cancellationToken)
    {
        var id = userService.GetId()!.Value;
        var query = new GetSingleUserQuery { Id = id };
        
        var user = await sender.Send(query, cancellationToken);

        var response = mapster.Map<UserResponse>(user);
        
        return Ok(response);
    }

    [HttpGet(ApiRoutes.Users.GetMany)]
    [OutputCache(PolicyName = OutputCache.Users.PolicyName)]
    [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsersResponse>> GetMany(
        [FromQuery] GetUsersRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userService.GetId();
        var query = new GetUsersQuery
        {
            Query = request.Q,
            Page = request.Page,
            Limit = request.PageSize,
            UserId = userId ?? null
        };
        
        var users = await sender.Send(query, cancellationToken);
        
        var response = mapster.Map<UsersResponse>(users);
        
        return Ok(response);
    }

    [HttpGet(ApiRoutes.Users.GetById)]
    [OutputCache(PolicyName = OutputCache.Users.PolicyName)]
    [ProducesResponseType(typeof(ExtendedUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExtendedUserResponse>> GetById(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleUserQuery { Id = userId };
        var user = await sender.Send(query, cancellationToken);
        
        var response = mapster.Map<ExtendedUserResponse>(user);
        
        return Ok(response);
    }
    
    #endregion Get endpoints

    #region Delete endpoints
    
    [HttpDelete(ApiRoutes.Users.Delete)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = userService.GetId()!.Value };
        await sender.Send(command, cancellationToken);

        await HttpContext.SignOutAsync();

        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        return NoContent();
    }
    
    #endregion Delete endpoints

    #region Put endpoints

    #region Authorized
    
    [HttpPut(ApiRoutes.Users.UpdateEmail)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmail(
        [FromBodyOrDefault] UpdateEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmailCommand
        {
            Id = userService.GetId()!.Value,
            Email = request.Email
        };

        await sender.Send(command, cancellationToken);

        userService.ChangeEmail(request.Email);

        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        return NoContent();
    }

    [HttpPut(ApiRoutes.Users.UpdateName)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateName(
        [FromBodyOrDefault] UpdateNameRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNameCommand
        {
            UserId = userService.GetId()!.Value,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName
        };

        await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);

        return NoContent();
    }

    [HttpPut(ApiRoutes.Users.UpdateAvatar)]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> UpdateAvatar(
        [FromForm] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        var command = new InsertAvatarCommand
        {
            Id = userService.GetId()!.Value,
            Image = request.Image
        };

        var result = await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        var response = mapster.Map<UserResponse>(result);
        
        return CreatedAtAction(nameof(GetById), new { userId = result.Id }, response);
    }
    
    #endregion Authorized
    
    #region Privileged
    
    [HttpPut(ApiRoutes.Users.UpdateRole)]
    [Authorize(Policies.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRole(
        [FromBodyOrDefault] ChangeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = userService.GetId()!.Value,
            Role = request.Role,
            UserId = request.UserId
        };

        await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);

        return NoContent();
    }
    
    #endregion Priveleged
    
    #endregion Put endpoints
    
    #endregion Users endpoints
    
    #region Followers
    
    [HttpPut(ApiRoutes.Users.AddRemoveFollower)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRemoveFollower(
        [FromRoute] Guid followingId,
        CancellationToken cancellationToken)
    {
        var command = new AddRemoveFollowerCommand
        {
            UserId = followingId,
            FollowerId = userService.GetId()!.Value
        };

        await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);

        return NoContent();
    }
    
    [HttpGet(ApiRoutes.Users.GetFollowers)]
    [OutputCache(PolicyName = OutputCache.Users.PolicyName)]
    [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsersResponse>> GetFollowers(
        [FromQuery] GetFollowersRequest request,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var currentUser = userService.GetId();
        var query = new GetUserRelationshipsQuery
        {
            Query = request.Query,
            Page = request.Page,
            Limit = request.PageSize,
            CurrentUserId = currentUser,
            UserId = userId,
            RelationshipType = RelationshipType.Followers
        };
        
        var users = await sender.Send(query, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        var response = mapster.Map<UsersResponse>(users);
        
        return Ok(response);
    }
    
    [HttpGet(ApiRoutes.Users.GetFollowing)]
    [OutputCache(PolicyName = OutputCache.Users.PolicyName)]
    [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsersResponse>> GetFollowing(
        [FromQuery] GetFollowingRequest request,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var currentUser = userService.GetId();
        var query = new GetUserRelationshipsQuery
        {
            Query = request.Query,
            Page = request.Page,
            Limit = request.PageSize,
            CurrentUserId = currentUser,
            UserId = userId,
            RelationshipType = RelationshipType.Following
        };
        var users = await sender.Send(query, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        var response = mapster.Map<UsersResponse>(users);
        
        return Ok(response);
    }
    
    #endregion Followers
}
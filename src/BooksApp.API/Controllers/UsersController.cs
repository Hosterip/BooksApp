using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Roles.Commands.UpdateRole;
using BooksApp.Application.Users.Commands.AddRemoveFollower;
using BooksApp.Application.Users.Commands.DeleteUser;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Application.Users.Commands.UpdateName;
using BooksApp.Application.Users.Queries.GetSingleUser;
using BooksApp.Application.Users.Queries.GetUserRelationships;
using BooksApp.Application.Users.Queries.GetUsers;
using BooksApp.Application.Users.Results;
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

public class UsersController : ApiController
{
    private readonly ISender _sender;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IMapper _mapster;

    public UsersController(ISender sender, IOutputCacheStore outputCacheStore, IMapper mapster)
    {
        _sender = sender;
        _outputCacheStore = outputCacheStore;
        _mapster = mapster;
    }

    #region Users endpoints

    #region Get endpoints
    
    [HttpGet(ApiRoutes.Users.GetMe)]
    [Authorize]
    [ProducesResponseType(typeof(ExtendedUserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExtendedUserResponse>> GetMe(
        CancellationToken cancellationToken)
    {
        var id = HttpContext.GetId()!.Value;
        var query = new GetSingleUserQuery { Id = id };
        
        var user = await _sender.Send(query, cancellationToken);

        var response = _mapster.Map<ExtendedUserResponse>(user);
        
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
        var userId = HttpContext.GetId();
        var query = new GetUsersQuery
        {
            Query = request.Q,
            Page = request.Page,
            Limit = request.PageSize,
            UserId = userId ?? null
        };
        
        var users = await _sender.Send(query, cancellationToken);
        
        var response = _mapster.Map<UsersResponse>(users);
        
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
        var user = await _sender.Send(query, cancellationToken);
        
        var response = _mapster.Map<ExtendedUserResponse>(user);
        
        return Ok(response);
    }
    
    #endregion Get endpoints

    #region Delete endpoints
    
    [HttpDelete(ApiRoutes.Users.Delete)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = HttpContext.GetId()!.Value };
        await _sender.Send(command, cancellationToken);

        await HttpContext.SignOutAsync();

        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        return Ok();
    }
    
    #endregion Delete endpoints

    #region Put endpoints

    #region Authorized
    
    [HttpPut(ApiRoutes.Users.UpdateEmail)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmail(
        [FromBodyOrDefault] UpdateEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmailCommand
        {
            Id = HttpContext.GetId()!.Value,
            Email = request.Email
        };

        await _sender.Send(command, cancellationToken);

        HttpContext.ChangeEmail(request.Email);

        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        return Ok();
    }

    [HttpPut(ApiRoutes.Users.UpdateName)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateName(
        [FromBodyOrDefault] UpdateNameRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNameCommand
        {
            UserId = HttpContext.GetId()!.Value,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName
        };

        await _sender.Send(command, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);

        return Ok();
    }

    [HttpPut(ApiRoutes.Users.UpdateAvatar)]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> UpdateAvatar(
        [FromBodyOrDefault] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        var command = new InsertAvatarCommand
        {
            Id = HttpContext.GetId()!.Value,
            Image = request.Image
        };

        var result = await _sender.Send(command, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        var response = _mapster.Map<UserResponse>(result);
        
        return CreatedAtAction(nameof(GetById), new { userId = result.Id }, response);
    }
    
    #endregion Authorized
    
    #region Privileged
    
    [HttpPut(ApiRoutes.Users.UpdateRole)]
    [Authorize(Policies.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRole(
        [FromBodyOrDefault] ChangeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = HttpContext.GetId()!.Value,
            Role = request.Role,
            UserId = request.UserId
        };

        await _sender.Send(command, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);

        return Ok();
    }
    
    #endregion Priveleged
    
    #endregion Put endpoints
    
    #endregion Users endpoints
    
    #region Followers
    
    [HttpPut(ApiRoutes.Users.AddFollower)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRemoveFollower(
        [FromRoute] Guid followingId,
        CancellationToken cancellationToken)
    {
        var command = new AddRemoveFollowerCommand
        {
            UserId = followingId,
            FollowerId = HttpContext.GetId()!.Value
        };

        await _sender.Send(command, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);

        return Ok();
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
        var currentUser = HttpContext.GetId();
        var query = new GetUserRelationshipsQuery
        {
            Query = request.Query,
            Page = request.Page,
            Limit = request.PageSize,
            CurrentUserId = currentUser,
            UserId = userId,
            RelationshipType = RelationshipType.Followers
        };
        
        var users = await _sender.Send(query, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        var response = _mapster.Map<UsersResponse>(users);
        
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
        var currentUser = HttpContext.GetId();
        var query = new GetUserRelationshipsQuery
        {
            Query = request.Query,
            Page = request.Page,
            Limit = request.PageSize,
            CurrentUserId = currentUser,
            UserId = userId,
            RelationshipType = RelationshipType.Following
        };
        var users = await _sender.Send(query, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Users.Tag, cancellationToken);
        
        var response = _mapster.Map<UsersResponse>(users);
        
        return Ok(response);
    }
    
    #endregion Followers
}
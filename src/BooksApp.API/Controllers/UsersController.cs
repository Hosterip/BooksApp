using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Commands.AddRemoveFollower;
using BooksApp.Application.Users.Commands.DeleteUser;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Application.Users.Commands.UpdateName;
using BooksApp.Application.Users.Queries.GetSingleUser;
using BooksApp.Application.Users.Queries.GetUserFollowers;
using BooksApp.Application.Users.Queries.GetUsers;
using BooksApp.Application.Users.Results;
using BooksApp.Contracts.Requests.Users;
using BooksApp.Contracts.Responses.Errors;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiRoutes.Users.GetMe)]
    [Authorize]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResult>> GetMe(
        CancellationToken cancellationToken)
    {
        var id = HttpContext.GetId()!.Value;
        var query = new GetSingleUserQuery { Id = id };
        var user = await _sender.Send(query, cancellationToken);
        return Ok(user);
    }

    [HttpGet(ApiRoutes.Users.GetMany)]
    [ProducesResponseType(typeof(PaginatedArray<UserResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedArray<UserResult>>> GetMany(
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
        return Ok(users);
    }

    [HttpGet(ApiRoutes.Users.GetById)]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResult>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleUserQuery { Id = id };
        var user = await _sender.Send(query, cancellationToken);
        return Ok(user);
    }

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

        return Ok();
    }

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

        return Ok();
    }

    [HttpPut(ApiRoutes.Users.UpdateAvatar)]
    [Authorize]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResult>> UpdateAvatar(
        [FromBodyOrDefault] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        var command = new InsertAvatarCommand
        {
            Id = HttpContext.GetId()!.Value,
            Image = request.Image
        };

        var result = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    // Followers
    
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

        return Ok();
    }
    
    [HttpGet(ApiRoutes.Users.GetFollowers)]
    [ProducesResponseType(typeof(PaginatedArray<UserResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedArray<UserResult>>> GetMany(
        [FromQuery] GetUsersRequest request,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var currentUser = HttpContext.GetId();
        var query = new GetUserFollowersQuery
        {
            Query = request.Q,
            Page = request.Page,
            Limit = request.PageSize,
            CurrentUserId = currentUser,
            UserId = userId
        };
        var users = await _sender.Send(query, cancellationToken);
        return Ok(users);
    }
}
using Contracts.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.InsertAvatar;
using PostsApp.Application.Users.Commands.UpdateEmail;
using PostsApp.Application.Users.Commands.UpdateName;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.User;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiRoutes.Users.GetMe)]
    public async Task<IActionResult> GetMe(
        CancellationToken cancellationToken)
    {
        var id = new Guid(HttpContext.GetId()!);
        var query = new GetSingleUserQuery { Id = id };
        var user = await _sender.Send(query, cancellationToken);
        return Ok(user);
    }

    [HttpGet(ApiRoutes.Users.GetMany)]
    public async Task<IActionResult> GetMany(
        [FromQuery] GetUsersRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery
        {
            Query = request.Q,
            Page = request.Page,
            Limit = request.Limit
        };
        var users = await _sender.Send(query, cancellationToken);
        return Ok(users);
    }

    [HttpGet(ApiRoutes.Users.GetById)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleUserQuery { Id = id };
        var user = await _sender.Send(query, cancellationToken);
        return Ok(user);
    }

    [HttpDelete(ApiRoutes.Users.Delete)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = new Guid(HttpContext.GetId()!) };
        await _sender.Send(command, cancellationToken);

        await HttpContext.SignOutAsync();

        return Ok("User was deleted");
    }

    [HttpPut(ApiRoutes.Users.UpdateEmail)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateEmail(
        [FromBodyOrDefault] UpdateEmail request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmailCommand
        {
            Id = new Guid(HttpContext.GetId()!),
            Email = request.Email
        };

        await _sender.Send(command, cancellationToken);

        HttpContext.ChangeEmail(request.Email);

        return Ok("Email was updated");
    }

    [HttpPut(ApiRoutes.Users.UpdateName)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateName(
        [FromBodyOrDefault] UpdateName request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNameCommand
        {
            UserId = new Guid(HttpContext.GetId()!),
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName
        };

        await _sender.Send(command, cancellationToken);

        return Ok("Name was updated");
    }

    [HttpPut(ApiRoutes.Users.UpdateAvatar)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateAvatar(
        [FromBodyOrDefault] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        var command = new InsertAvatarCommand
        {
            Id = new Guid(HttpContext.GetId()!),
            Image = request.Image
        };

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}
using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Users.Commands.DeleteUser;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Application.Users.Commands.UpdateName;
using BooksApp.Application.Users.Queries.GetSingleUser;
using BooksApp.Application.Users.Queries.GetUsers;
using BooksApp.Contracts.Requests.Users;
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
    public async Task<IActionResult> GetMe(
        CancellationToken cancellationToken)
    {
        var id = HttpContext.GetId()!.Value;
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
        var command = new DeleteUserCommand { Id = HttpContext.GetId()!.Value };
        await _sender.Send(command, cancellationToken);

        await HttpContext.SignOutAsync();

        return Ok();
    }

    [HttpPut(ApiRoutes.Users.UpdateEmail)]
    [Authorize(Policies.Authorized)]
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
    [Authorize(Policies.Authorized)]
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
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateAvatar(
        [FromBodyOrDefault] InsertAvatarRequest request,
        CancellationToken cancellationToken)
    {
        var command = new InsertAvatarCommand
        {
            Id = HttpContext.GetId()!.Value,
            Image = request.Image
        };

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}
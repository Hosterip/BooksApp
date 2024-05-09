using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.UpdateUsername;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.User;
using PostsApp.Contracts.Responses.User;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Policies.Authorized)]
    public IActionResult GetUser()
    {
        var username = HttpContext.GetUsername()!;
        var role = HttpContext.GetRole()!;
        var id = HttpContext.GetId();
        return Ok(new UserResponse { Id = id, Username = username, Role = role });
    }

    [HttpGet("many")]
    public async Task<IActionResult> GetManyUsers(int? page, int? limit, string? q, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery { Query = q, Page = page, Limit = limit };
        var users = await _sender.Send(query, cancellationToken);
        return Ok(users);
    }

    [HttpGet("single/{id:int}")]
    public async Task<IActionResult> GetUserByUsername(int id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetSingleUserQuery { Id = id };
            var user = await _sender.Send(query, cancellationToken);
            return Ok(user);
        }
        catch (UserException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteUserCommand { Id = HttpContext.GetId() };
            await _sender.Send(command, cancellationToken);
        }
        catch (UserException ex)
        {
            return BadRequest(ex.Message);
        }

        await HttpContext.SignOutAsync();

        return Ok("User was deleted");
    }

    [HttpPut("username")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> UpdateUsername([FromForm] [FromBody] UpdateUsername request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUsernameCommand { Id = HttpContext.GetId(), NewUsername = request.NewUsername };

        await _sender.Send(command, cancellationToken);

        HttpContext.ChangeUsername(request.NewUsername);

        return Ok("Username was updated");
    }
}
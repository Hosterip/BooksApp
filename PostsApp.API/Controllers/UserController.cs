using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Shared.Extensions;
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
    public IActionResult GetUser()
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");

        return Ok(new DefaultUserResponse { username = HttpContext.Session.GetUserInSession()! });
    }

    [HttpGet("many/{page:int}")]
    public async Task<IActionResult> GetManyUsers(int page, int? limit, string? q, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery { Query = q, Page = page, Limit = limit };
        var users = await _sender.Send(query, cancellationToken);
        return Ok(users);
    }

    [HttpGet("single/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetSingleUserQuery { Username = username };
            var user = await _sender.Send(query, cancellationToken);
            return Ok(user);
        }
        catch (UserException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");

        var command = new DeleteUserCommand { username = HttpContext.Session.GetUserInSession()! };
        await _sender.Send(command, cancellationToken);

        HttpContext.Session.RemoveUserInSession();

        return Ok("User was deleted");
    }
}
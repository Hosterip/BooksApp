using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.UpdateUsername;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.User;
using PostsApp.Common.Contracts.Responses.User;
using PostsApp.Common.Extensions;
using PostsApp.Domain.Exceptions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class ImageRequest
{
    public IFormFile Image { get; set; }
}

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
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
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

    [HttpPost("avatar")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> AddAvatar([FromBodyOrDefault] ImageRequest request, CancellationToken cancellationToken)
    {
    //     var command = new CreateImageCommand {
    //         Image = request.Image
    //     };
    //
    //     await _sender.Send(command, cancellationToken);

        return Ok("all good");
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
    public async Task<IActionResult> UpdateUsername([FromBodyOrDefault] UpdateUsername request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUsernameCommand { Id = HttpContext.GetId(), NewUsername = request.NewUsername };

        await _sender.Send(command, cancellationToken);

        HttpContext.ChangeUsername(request.NewUsername);

        return Ok("Username was updated");
    }
}
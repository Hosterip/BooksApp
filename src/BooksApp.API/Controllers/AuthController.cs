using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Auth.Commands.Register;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Auth;
using PostsApp.Common.Contracts.Responses.User;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("Register")]
    [Authorize(Policy = Policies.NotAuthorized)]
    public async Task<IActionResult> RegisterPost([FromBodyOrDefault] AuthPostRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand { Username = request.Username, Password = request.Password };
        var user = await _sender.Send(command, cancellationToken);
        await HttpContext.Login(user.Id, user.Username, user.Role, user.SecurityStamp);
        return StatusCode(201, new UserResponse
        {
            Id = user.Id.ToString(),
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPost("Login")]
    [Authorize(Policy = Policies.NotAuthorized)]
    public async Task<IActionResult> LoginPost(
        [FromBodyOrDefault] AuthPostRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserQuery { Username = request.Username, Password = request.Password };
        var user = await _sender.Send(command, cancellationToken);
        await HttpContext.Login(user.Id, user.Username, user.Role, user.SecurityStamp);
        return Ok(new UserResponse
        {
            Id = user.Id.ToString(),
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPut("change")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> UpdatePassword(
        [FromBodyOrDefault] AuthUpdatePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand
        {
            NewPassword = request.NewPassword,
            OldPassword = request.OldPassword,
            Id = new Guid(HttpContext.GetId()!)
        };
        var result = await _sender.Send(command, cancellationToken);
        HttpContext.ChangeSecurityStamp(result.SecurityStamp);
        return Ok("Operation succeeded");
    }

    [HttpPost("Logout")]
    [Authorize(Policy = Policies.Authorized)]
    public IActionResult LogoutPost()
    {
        HttpContext.SignOutAsync();
        return Ok("You've been signed out");
    }
}
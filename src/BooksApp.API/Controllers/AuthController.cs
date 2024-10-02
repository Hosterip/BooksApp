using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Auth.Commands.Register;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Auth;
using PostsApp.Common.Contracts.Responses.User;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("Register")]
    [Authorize(Policy = Policies.NotAuthorized)]
    public async Task<IActionResult> RegisterPost([FromBodyOrDefault] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Email = request.Email,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Password = request.Password,
        };
        var user = await _sender.Send(command, cancellationToken);
        var createDefaultBookshelves = new CreateDefaultBookshelvesCommand
        {
            UserId = Guid.Parse(user.Id)
        };
        await _sender.Send(createDefaultBookshelves, cancellationToken);
        await HttpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return StatusCode(201, user.Adapt<UserResponse>());
    }

    [HttpPost("Login")]
    [Authorize(Policy = Policies.NotAuthorized)]
    public async Task<IActionResult> LoginPost(
        [FromBodyOrDefault] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserQuery { Email = request.Email, Password = request.Password };
        var user = await _sender.Send(command, cancellationToken);
        await HttpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return Ok(user.Adapt<UserResponse>());
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
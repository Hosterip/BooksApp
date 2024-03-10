using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Auth.Commands.Register;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Auth;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Exceptions;

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
    public async Task<IActionResult> RegisterPost(AuthPostRequest request, CancellationToken cancellationToken)
    {
        if (HttpContext.IsAuthorized())
            return StatusCode(401, "You are already authorized");

        try
        {
            var command = new RegisterUserCommand{Username = request.Username, Password = request.Password };
            var user = await _sender.Send(command, cancellationToken);
            await HttpContext.Login(user.Username, user.Id, RoleConstants.Member);
            return StatusCode(201, user);
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginPost(
        AuthPostRequest request,
        CancellationToken cancellationToken)
    {
        if (HttpContext.IsAuthorized())
            return StatusCode(401, "You are already authorized");

        try
        {
            var command = new LoginUserQuery { Username = request.Username, Password = request.Password };
            var user = await _sender.Send(command, cancellationToken);
            await HttpContext.Login(user.Username, user.Id, user.Role ?? RoleConstants.Member);
            return Ok(user);
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("change")]
    public async Task<IActionResult> UpdatePassword(
        AuthUpdatePasswordRequest request,
        CancellationToken cancellationToken)
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");

        try
        {
            var command = new ChangePasswordCommand
            {
                NewPassword = request.NewPassword,
                OldPassword = request.OldPassword,
                Id = HttpContext.GetId()
            };
            await _sender.Send(command, cancellationToken);
            return Ok("Operation succeeded");
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Logout")]
    public IActionResult LogoutPost()
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401, "You are not authorized");
        HttpContext.SignOutAsync();
        return Ok("You've been signed out");
    }
    
}
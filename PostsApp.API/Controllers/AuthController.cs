using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Application.Auth.Commands.Register;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Contracts.Requests.Auth;
using PostsApp.Domain.Auth;
using PostsApp.Shared.Extensions;

namespace PostsApp.Controllers;

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
            return StatusCode(403, "You are already authorized");
        
        if (request.username.IsNullOrEmpty() || request.password.IsNullOrEmpty()) 
            return BadRequest("Please enter required data");
        if (request.username.Length > 255)
            return BadRequest("Username length must be less than 255");

        try
        {
            var command = new RegisterUserCommand{Username = request.username, Password = request.password};
            var user = await _sender.Send(command, cancellationToken);
            HttpContext.Session.SetUserInSession(user.username);
            return StatusCode(201, user);
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginPost(AuthPostRequest request, CancellationToken cancellationToken)
    {
        if (HttpContext.IsAuthorized())
            return StatusCode(403, "You are already authorized");
        
        if(request.username.IsNullOrEmpty() || request.password.IsNullOrEmpty()) 
            return BadRequest("Please enter required data");

        try
        {
            var command = new LoginUserQuery { Username = request.username, Password = request.password};
            var user = await _sender.Send(command, cancellationToken);
            HttpContext.Session.SetUserInSession(user.username);
            return Ok(user);
        }
        catch (AuthException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("Logout")]
    public IActionResult LogoutPost()
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(403, "You are not authorized");
        HttpContext.Session.RemoveUserInSession();
        return Ok("You've been signed out");
    }
    
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Application.Interfaces;
using PostsApp.Contracts.Requests.Auth;
using PostsApp.Domain.Auth;
using PostsApp.Domain.Exceptions;
using PostsApp.Shared.Extensions;

namespace PostsApp.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterPost(AuthPostRequest request)
    {
        if (HttpContext.IsAuthorized())
            return StatusCode(403, "You are already authorized");
        
        if (request.username.IsNullOrEmpty() || request.password.IsNullOrEmpty()) 
            return BadRequest("Please enter required data");
        if (request.username.Length > 255)
            return BadRequest("Username length must be less than 255 ");

        try
        {
            await _authService.Register(request.username, request.password);
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
        catch (SqlException)
        {
            return StatusCode(500, "Something went wrong!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a user.");
        }
        
        
        HttpContext.Session.SetUserInSession(request.username);
        
        return StatusCode(201, request.username);
    }

    [HttpPost("Login")]
    public IActionResult LoginPost(AuthPostRequest request)
    {
        if (HttpContext.IsAuthorized())
            return StatusCode(403, "You are already authorized");
        
        if(request.username.IsNullOrEmpty() || request.password.IsNullOrEmpty()) 
            return BadRequest("Please enter required data");

        try
        {
            _authService.Login(request.username, request.password);
        }
        catch (SqlException)
        {
            return StatusCode(500, "Something went wrong");
        }
        catch (AuthException)
        {
            return BadRequest("Username or password is wrong");
        }
        
        HttpContext.Session.SetUserInSession(request.username);
        
        return StatusCode(201, request.username); 
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
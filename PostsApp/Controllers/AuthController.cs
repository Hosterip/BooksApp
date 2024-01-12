using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Requests.Auth;
using PostsApp.Services.Auth;
using PostsApp.Shared.Extensions;

namespace PostsApp.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
            await _authService.Register(request);
        }
        catch (SqlException)
        {
            return StatusCode(500, "Something went wrong");
        }
        catch (BadHttpRequestException)
        {
            return BadRequest("Username is occupied");
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
            _authService.Login(request);
        }
        catch (SqlException)
        {
            return StatusCode(500, "Something went wrong");
        }
        catch (BadHttpRequestException)
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
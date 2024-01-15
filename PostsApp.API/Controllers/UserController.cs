using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PostsApp.Application.Services.Users;
using PostsApp.Shared.Extensions;
using PostsApp.Contracts.Responses.User;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
        
    [HttpGet]
    public IActionResult GetUser()
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401,"You are not authorized");

        return Ok(new DefaultUserResponse{username = HttpContext.Session.GetUserInSession()!});
    }
    
    [HttpGet("many/{page:int}")]
    public IActionResult GetManyUsers(int page)
    {
        try
        {
            string? query = Request.Query["q"];
            string? strLimit = Request.Query["limit"];
            int intLimit = !strLimit.IsNullOrEmpty() ? Convert.ToInt32(strLimit) : 10;
            return Ok(_userService.GetUsers(intLimit, page, query ?? ""));
        }
        catch (FormatException)
        {
            return BadRequest("Limit must be an integer");
        }
    }

    [HttpGet("{username}")]
    public IActionResult GetUserByUsername(string username)
    {
        try
        {
            return Ok(_userService.GetUserByUsername(username));
        }
        catch (UserException)
        {
            return BadRequest("User not found");
        }
    }
    
    

    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        if (!HttpContext.IsAuthorized())
            return StatusCode(401,"You are not authorized");
            
        await _userService.DeleteUser(HttpContext.Session.GetUserInSession()!);
        HttpContext.Session.RemoveUserInSession();

        return Ok("User were deleted");
    }
}
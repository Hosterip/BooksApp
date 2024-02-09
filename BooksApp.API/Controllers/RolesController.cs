using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Users.Commands.UpdateRole;
using PostsApp.Common.Extensions;
using PostsApp.Contracts.Requests.Role;
using PostsApp.Domain.Constants;

namespace PostsApp.Controllers;

[Route("roles")]
public class RolesController : Controller
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<String> roles = new List<String>();
        roles.Add(RoleConstants.Member);    
        roles.Add(RoleConstants.Author);    
        roles.Add(RoleConstants.Moderator);    
        roles.Add(RoleConstants.Admin);

        return Ok(roles);
    }
    
    [Authorize(Policy = "AdminOrModerator")]
    [HttpPut]
    public async Task<IActionResult> UpdateRole(ChangeRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = (int)HttpContext.GetId()!, 
            Role = request.Role, 
            UserId = request.UserId
        };

        await _sender.Send(command, cancellationToken);
        
        return Ok("Operation succeeded");
    }
}
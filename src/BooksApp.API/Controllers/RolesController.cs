using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Roles.Commands.UpdateRole;
using PostsApp.Application.Roles.Queries.GetRoles;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Role;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public class RolesController : ApiController
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    // Users endpoints
    [HttpPut(ApiRoutes.Users.UpdateRole)]
    [Authorize(Policies.Authorized)]
    public async Task<IActionResult> UpdateRole(
        [FromBodyOrDefault] ChangeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = new Guid(HttpContext.GetId()!),
            Role = request.Role,
            UserId = request.UserId
        };

        await _sender.Send(command, cancellationToken);

        return Ok("Operation succeeded");
    }


    [HttpGet(ApiRoutes.Users.GetRoles)]
    public async Task<IActionResult> GetRoles(
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await _sender.Send(command, cancellationToken);

        return Ok(roles);
    }
}
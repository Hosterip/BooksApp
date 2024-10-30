using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Roles;
using BooksApp.Application.Roles.Commands.UpdateRole;
using BooksApp.Application.Roles.Queries.GetRoles;
using BooksApp.Contracts.Requests.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class RolesController : ApiController
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    // Users endpoints
    [HttpPut(ApiRoutes.Users.UpdateRole)]
    [Authorize(Policies.Admin)]
    public async Task<IActionResult> UpdateRole(
        [FromBodyOrDefault] ChangeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = HttpContext.GetId()!.Value,
            Role = request.Role,
            UserId = request.UserId
        };

        await _sender.Send(command, cancellationToken);

        return Ok();
    }


    [HttpGet(ApiRoutes.Users.GetRoles)]
    public async Task<ActionResult<RoleResult[]>> GetRoles(
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await _sender.Send(command, cancellationToken);

        return Ok(roles);
    }
}
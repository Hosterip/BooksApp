using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Roles;
using BooksApp.Application.Roles.Commands.UpdateRole;
using BooksApp.Application.Roles.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class RolesController : ApiController
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet(ApiRoutes.Users.GetRoles)]
    [OutputCache]
    [ProducesResponseType(typeof(IEnumerable<RoleResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResult>>> GetRoles(
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await _sender.Send(command, cancellationToken);

        return Ok(roles);
    }
}
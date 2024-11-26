using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Roles;
using BooksApp.Application.Roles.Commands.UpdateRole;
using BooksApp.Application.Roles.Queries.GetRoles;
using BooksApp.Contracts.Roles;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class RolesController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapster;

    public RolesController(ISender sender, IMapper mapster)
    {
        _sender = sender;
        _mapster = mapster;
    }
    
    [HttpGet(ApiRoutes.Users.GetRoles)]
    [OutputCache]
    [ProducesResponseType(typeof(IEnumerable<RoleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResult>>> GetRoles(
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await _sender.Send(command, cancellationToken);

        var response = _mapster.Map<IEnumerable<RoleResponse>>(roles);
        
        return Ok(response);
    }
}
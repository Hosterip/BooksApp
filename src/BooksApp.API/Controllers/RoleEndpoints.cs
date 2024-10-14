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

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var roles = app.MapGroup("api/roles");

        roles.MapGet("", GetAll);

        roles.MapPut("", UpdateRole)
            .RequireAuthorization(Policies.Authorized);
    }
    
    [HttpGet]
    public static async Task<IResult> GetAll(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await sender.Send(command, cancellationToken);

        return Results.Ok(roles);
    }
    
    [Authorize(Policy = Policies.Authorized)]
    [HttpPut]
    public static async Task<IResult> UpdateRole(
        [FromBodyOrDefault]ChangeRoleRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = new Guid(httpContext.GetId()!), 
            Role = request.Role, 
            UserId = request.UserId
        };

        await sender.Send(command, cancellationToken);
        
        return Results.Ok("Operation succeeded");
    }
}
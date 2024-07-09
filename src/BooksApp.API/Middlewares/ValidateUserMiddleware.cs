using MediatR;
using Microsoft.AspNetCore.Authentication;
using PostsApp.Application.Auth.Queries.GetFullUser;
using PostsApp.Common.Extensions;

namespace PostsApp.Middlewares;

public class ValidateUserMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISender sender)
    {
        var id = context.GetId();
        var securityStamp = context.GetSecurityStamp();
        var role = context.GetRole();
        if (id != null && securityStamp != null)
        {
            var query = new GetFullUserQuery
            {
                UserId = Guid.Parse(id)
            };
            var result = await sender.Send(query);
            if (result is null || result.SecurityStamp.ToString() != securityStamp)
                await context.SignOutAsync();
            else if (result.Role.Name != role)
                context.ChangeRole(result.Role.Name);
        }

        await _next(context);
    }
}
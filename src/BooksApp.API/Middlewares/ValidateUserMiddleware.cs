using BooksApp.API.Common.Extensions;
using BooksApp.Application.Auth.Queries.ValidateUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace BooksApp.API.Middlewares;

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
            var query = new ValidateUserQuery
            {
                UserId = id.Value,
                SecurityStamp = securityStamp
            };
            var userRole = await sender.Send(query);
            if (userRole is null)
                await context.SignOutAsync();
            else if (userRole != role)
                context.ChangeRole(userRole);
        }

        await _next(context);
    }
}
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace PostsApp.Middlewares;

public class MyAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
            return;
        }

        // User must be authorized for failureReasons not to be null 
        var result = authorizeResult.AuthorizationFailure?.FailureReasons.FirstOrDefault();

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.Body
            .WriteAsync(Encoding.UTF8.GetBytes(result?.Message ?? "You don't have permission to access."));
    }
}
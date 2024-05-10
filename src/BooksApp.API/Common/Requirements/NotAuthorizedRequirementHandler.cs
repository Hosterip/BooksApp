using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PostsApp.Common.Requirements;

public class NotAuthorizedRequirement : IAuthorizationRequirement {}

public class NotAuthorizedRequirementHandler : AuthorizationHandler<NotAuthorizedRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        NotAuthorizedRequirement requirement)
    {
        if (!context.User.HasClaim(user => user.Type == ClaimTypes.NameIdentifier))
        {
            context.Succeed(requirement);
        }
        else
        {
            var reason = new AuthorizationFailureReason(this, "You are already authorized");
            context.Fail(reason);
        }

        return Task.CompletedTask;
    }
}
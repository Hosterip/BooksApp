using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PostsApp.Common.Requirements;

public class AuthorizedRequirement : AuthorizationHandler<AuthorizedRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AuthorizedRequirement requirement)
    {
        if (context.User.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
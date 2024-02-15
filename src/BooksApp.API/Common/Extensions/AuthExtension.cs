using System.Security.Claims;

namespace PostsApp.Common.Extensions;

public static class AuthExtension
{
    public static bool IsAuthorized(this HttpContext httpContext)
    {
        return httpContext.User.HasClaim(user => user.Value != ClaimTypes.NameIdentifier);
    }
}
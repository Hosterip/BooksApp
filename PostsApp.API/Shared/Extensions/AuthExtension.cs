
using Microsoft.IdentityModel.Tokens;

namespace PostsApp.Shared.Extensions;

public static class AuthExtension
{
    public static bool IsAuthorized(this HttpContext httpContext)
    {
        return !httpContext.Session.GetUserInSession().IsNullOrEmpty();
    }
}
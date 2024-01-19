
using Microsoft.IdentityModel.Tokens;
using PostsApp.Common.Extensions;

namespace PostsApp.Shared.Extensions;

public static class AuthExtension
{
    public static bool IsAuthorized(this HttpContext httpContext)
    {
        return !httpContext.Session.GetUserInSession().IsNullOrEmpty();
    }
}
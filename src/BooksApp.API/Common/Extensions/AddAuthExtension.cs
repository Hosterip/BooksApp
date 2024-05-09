using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PostsApp.Common.Constants;
using PostsApp.Common.Requirements;
using PostsApp.Domain.Constants;
using PostsApp.Middlewares;

namespace PostsApp.Common.Extensions;

public static class AddAuthExtension
{
    public static void AddAuth(this IServiceCollection serviceCollection)
    {
        // Overriding default AuthorizationMiddlewareResultHandler
        serviceCollection.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();

        // Adding a cookie for auth
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
            });
        // Adding policies
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.Admin, policy => policy.RequireRole([RoleConstants.Admin]));
            
            options.AddPolicy(Policies.Moderator, policy => policy.RequireRole([RoleConstants.Moderator]));
            
            options.AddPolicy(Policies.Authorized, policy =>
                policy.Requirements.Add(AuthRequirements.Authorized));
            
            options.AddPolicy(Policies.NotAuthorized, policy => 
                policy.Requirements.Add(AuthRequirements.NotAuthorized));
        });
    }
}
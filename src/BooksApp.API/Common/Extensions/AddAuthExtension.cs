using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PostsApp.Common.Constants;
using PostsApp.Common.Requirements;
using PostsApp.Domain.Common.Constants;
using PostsApp.Middlewares;

namespace PostsApp.Common.Extensions;

public static class AddAuthExtension
{
    public static void AddAuth(this IServiceCollection serviceCollection)
    {
        // Overriding default AuthorizationMiddlewareResultHandler
        serviceCollection.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();
        serviceCollection.AddSingleton<IAuthorizationHandler, AuthorizedRequirementHandler>();
        serviceCollection.AddSingleton<IAuthorizationHandler, NotAuthorizedRequirementHandler>();
        // Adding a cookie for auth
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.SlidingExpiration = true;
            });
        // Adding policies
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.Admin, policy => policy.RequireRole([RoleNames.Admin]));
            
            options.AddPolicy(Policies.Moderator, policy => policy.RequireRole([RoleNames.Moderator]));
            
            options.AddPolicy(Policies.AdminOrModerator, policy => policy.RequireRole([RoleNames.Admin, RoleNames.Moderator]));
            
            options.AddPolicy(Policies.Author, policy => policy.RequireRole([RoleNames.Author]));
            
            options.AddPolicy(Policies.Authorized, policy =>
                policy.Requirements.Add(AuthRequirements.Authorized));
            
            options.AddPolicy(Policies.NotAuthorized, policy => 
                policy.Requirements.Add(AuthRequirements.NotAuthorized));
        });
    }
}
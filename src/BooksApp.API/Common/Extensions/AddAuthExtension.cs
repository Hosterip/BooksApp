using System.Security.Claims;
using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Requirements;
using BooksApp.API.Middlewares;
using BooksApp.Domain.Common.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace BooksApp.API.Common.Extensions;

public static class AddAuthExtension
{
    public static void AddAuth(this IServiceCollection serviceCollection)
    {
        // Overriding default AuthorizationMiddlewareResultHandler
        serviceCollection.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();
        serviceCollection.AddSingleton<IAuthorizationHandler, NotAuthorizedRequirementHandler>();
        // Adding a cookie for auth
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
        // Adding policies
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.Admin, policy => policy.RequireRole([RoleNames.Admin]));

            options.AddPolicy(Policies.Moderator, policy => policy.RequireAssertion(x =>
                x.User.HasClaim(ClaimTypes.Role, RoleNames.Admin) ||
                x.User.HasClaim(ClaimTypes.Role, RoleNames.Moderator)
            ));

            options.AddPolicy(Policies.Author, policy => policy.RequireRole([RoleNames.Author]));

            options.AddPolicy(Policies.NotAuthorized, policy =>
                policy.Requirements.Add(AuthRequirements.NotAuthorized));
        });
    }
}
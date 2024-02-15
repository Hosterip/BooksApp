using Microsoft.AspNetCore.Authentication.Cookies;
using PostsApp.Domain.Constants;

namespace PostsApp.Common.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddAuth(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
            });
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole([RoleConstants.Admin]));
            options.AddPolicy("Moderator", policy => policy.RequireRole([RoleConstants.Moderator]));
            options.AddPolicy("AdminOrModerator", policy => policy.RequireRole([RoleConstants.Admin, RoleConstants.Moderator]));
        });
    }
}
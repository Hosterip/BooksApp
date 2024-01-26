using Microsoft.AspNetCore.Authentication.Cookies;

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
    }
}
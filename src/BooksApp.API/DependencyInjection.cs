using System.Security.Claims;
using BooksApp.API.Common;
using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Requirements;
using BooksApp.API.Middlewares;
using BooksApp.Contracts.Requests.Books;
using BooksApp.Contracts.Requests.Users;
using BooksApp.Domain.Common.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, string corsPolicy)
    {
        services
            .AddMyControllers()
            .AddCorsPolicy(corsPolicy)
            .AddAuth()
            .AddMyOutputCache();
        return services;
    }

    private static IServiceCollection AddMyControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.ModelBinderProviders.InsertBodyOrDefaultBinding();
        });
        return services;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        // Overriding default AuthorizationMiddlewareResultHandler
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();
        services.AddSingleton<IAuthorizationHandler, NotAuthorizedRequirementHandler>();
        // Adding a cookie for auth
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
        // Adding policies
        services.AddAuthorization(options =>
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
        return services;
    }
    
    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policyBuilder =>
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(hostName => true));
        });
        return services;
    }

    private static IServiceCollection AddMyOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(x =>
        {
            x.AddBasePolicy(c => c.Cache());
            x.AddPolicy(OutputCache.Books.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByRouteValue(new []
                    {
                        "userId",
                        "bookId"
                    })
                    .SetVaryByQuery(new []
                    {
                        nameof(GetBooksRequest.Title),
                        nameof(GetBooksRequest.GenreId),
                        nameof(GetBooksRequest.Page),
                        nameof(GetBooksRequest.PageSize),
                    })
                    .Tag(OutputCache.Books.Tag);
            });
            
            x.AddPolicy(OutputCache.Users.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByRouteValue(new []
                    {
                        "userId"
                    })
                    .SetVaryByQuery(new []
                    {
                        nameof(GetUsersRequest.Q),
                        nameof(GetUsersRequest.Page),
                        nameof(GetUsersRequest.PageSize),
                    })
                    .Tag(OutputCache.Users.Tag);
            });
        });
        return services;
    }
}
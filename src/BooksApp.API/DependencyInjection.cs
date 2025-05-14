using System.Reflection;
using System.Security.Claims;
using BooksApp.API.Common;
using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Requirements;
using BooksApp.API.Common.Services;
using BooksApp.API.Middlewares;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Contracts;
using BooksApp.Contracts.Books;
using BooksApp.Contracts.Users;
using BooksApp.Domain.Common.Constants;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, string corsPolicy)
    {
        services
            .AddMapping()
            .AddMyControllers()
            .AddCorsPolicy(corsPolicy)
            .AddAuth()
            .AddMyOutputCache()
            .AddHttpContextAccessor();

        services.AddScoped<IUserService, UserService>();

        return services;
    }

    private static IServiceCollection AddMyControllers(this IServiceCollection services)
    {
        services.AddControllers(options => { options.ModelBinderProviders.InsertBodyOrDefaultBinding(); });
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
            options.AddPolicy(Policies.Admin, policy => policy.RequireRole(RoleNames.Admin));

            options.AddPolicy(Policies.Moderator, policy => policy.RequireAssertion(x =>
                x.User.HasClaim(ClaimTypes.Role, RoleNames.Admin) ||
                x.User.HasClaim(ClaimTypes.Role, RoleNames.Moderator)
            ));

            options.AddPolicy(Policies.Author, policy => policy.RequireRole(RoleNames.Author));

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
                    .SetIsOriginAllowed(_ => true));
        });
        return services;
    }

    private static IServiceCollection AddMyOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(x =>
        {
            x.AddBasePolicy(c => c.Cache());

            #region Books policy

            x.AddPolicy(OutputCache.Books.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByRouteValue([
                        "userId",
                        "bookId"
                    ])
                    .SetVaryByQuery([
                        nameof(GetBooksRequest.Title),
                        nameof(GetBooksRequest.GenreId),
                        nameof(PagedRequest.Page),
                        nameof(PagedRequest.PageSize)
                    ])
                    .Tag(OutputCache.Books.Tag);
            });

            #endregion Books policy

            #region Users policy

            x.AddPolicy(OutputCache.Users.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByRouteValue([
                        "userId",
                        "followingId"
                    ])
                    .SetVaryByQuery([
                        nameof(GetUsersRequest.Q),
                        nameof(GetFollowersRequest.Query),
                        nameof(GetFollowingRequest.Query),
                        nameof(PagedRequest.Page),
                        nameof(PagedRequest.PageSize)
                    ])
                    .Tag(OutputCache.Users.Tag);
            });

            #endregion Users policy

            #region Bookshelves policy

            x.AddPolicy(OutputCache.Bookshelves.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByRouteValue([
                        "userId",
                        "bookId",
                        "idOrName",
                        "bookshelfId"
                    ])
                    .SetVaryByQuery([
                        nameof(PagedRequest.Page),
                        nameof(PagedRequest.PageSize)
                    ])
                    .Tag(OutputCache.Bookshelves.Tag);
            });

            #endregion Bookshelves policy

            #region Reviews policy

            x.AddPolicy(OutputCache.Reviews.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByRouteValue([
                        "bookId"
                    ])
                    .SetVaryByQuery([
                        nameof(PagedRequest.Page),
                        nameof(PagedRequest.PageSize)
                    ])
                    .Tag(OutputCache.Reviews.Tag);
            });

            #endregion Reviews policy

            #region Genres policy

            x.AddPolicy(OutputCache.Genres.PolicyName, c =>
            {
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(10))
                    .Tag(OutputCache.Genres.Tag);
            });

            #endregion Genres policy
        });

        return services;
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);

        return services;
    }
}
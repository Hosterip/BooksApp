using Microsoft.Extensions.DependencyInjection;
using PostsApp.Application.Services.Auth;
using PostsApp.Application.Services.Posts;
using PostsApp.Application.Services.Users;

namespace PostsApp.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPostsService, PostsService>();
        return services;
    }
}
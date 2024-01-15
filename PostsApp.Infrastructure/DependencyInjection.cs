using Microsoft.Extensions.DependencyInjection;
using PostsApp.Application.Interfaces;
using PostsApp.Infrastructure.DB;
using PostsApp.Infrastructure.Services;

namespace PostsApp.Infrastructure;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPostsService, PostsService>();
        return services;
    }
}
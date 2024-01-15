using Microsoft.Extensions.DependencyInjection;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>();
        return services;
    }
}
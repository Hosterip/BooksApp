using Microsoft.Extensions.DependencyInjection;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;
    }
}
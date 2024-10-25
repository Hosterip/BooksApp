using Microsoft.Extensions.DependencyInjection;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Infrastructure.Data;
using PostsApp.Infrastructure.Files;
using PostsApp.Infrastructure.Implementation;

namespace PostsApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IImageFileBuilder, ImageFileBuilder>();
        
        return services;
    }
}
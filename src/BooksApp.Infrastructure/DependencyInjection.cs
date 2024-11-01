using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Interfaces;
using BooksApp.Infrastructure.Authentication;
using BooksApp.Infrastructure.Data;
using BooksApp.Infrastructure.Files;
using BooksApp.Infrastructure.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace BooksApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IImageFileBuilder, ImageFileBuilder>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
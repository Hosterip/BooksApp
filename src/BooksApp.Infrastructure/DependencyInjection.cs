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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string imagePath)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IImageFileBuilder>(new ImageFileBuilder(imagePath));

        return services;
    }
}
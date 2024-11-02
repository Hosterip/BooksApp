using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Interfaces;
using BooksApp.Infrastructure.Authentication;
using BooksApp.Infrastructure.Data;
using BooksApp.Infrastructure.Files;
using BooksApp.Infrastructure.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string imagesPath,
        string dbHost,
        string dbName,
        string dbPassword)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                $"Server={dbHost};Database={dbName};User Id=sa;Password={dbPassword};Trusted_Connection=False;TrustServerCertificate=True");
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IImageFileBuilder>(new ImageFileBuilder(imagesPath));

        return services;
    }
}
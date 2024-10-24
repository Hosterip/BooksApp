using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PostsApp.Controllers;

namespace PostsApp.Common.Extensions;

public static class MapEndpointsExtension
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly  assembly)
    {
        ServiceDescriptor[] serviceDescriptors =
            assembly
                .GetTypes()
                .Where(type =>
                    type is { IsAbstract: false, IsInterface: false } &&
                    typeof(IEndpoint).IsAssignableFrom(type)
                )
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();
        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
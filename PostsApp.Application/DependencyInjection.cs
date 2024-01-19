using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PostsApp.Application.Common.Behavior;

namespace PostsApp.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        } );
        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
        return services;
    }
}
namespace PostsApp.Common.Extensions;

public static class AddCorsExtension
{
    public static void AddCorsPolicy(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: policyName, policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
            });
        });
    }
}
using Microsoft.AspNetCore.Mvc;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public class ErrorEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Error.ErrorHandler, ErrorHandler);
    }
    
    public static IResult ErrorHandler()
    {
        return Results.Problem();
    }
}
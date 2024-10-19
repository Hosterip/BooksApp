using Microsoft.AspNetCore.Mvc;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public static class ErrorEndpoints 
{
    public static void MapErrorEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Error.ErrorHandler, ErrorHandler);
    }
    
    public static IResult ErrorHandler()
    {
        return Results.Problem();
    }
}
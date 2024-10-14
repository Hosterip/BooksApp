using Microsoft.AspNetCore.Mvc;

namespace PostsApp.Controllers;

public static class ErrorEndpoints 
{
    public static void MapErrorEndpoints(this IEndpointRouteBuilder app)
    {
        var error = app.MapGroup("api/error");
        error.MapGet("", ErrorHandler);
    }
    
    public static IResult ErrorHandler()
    {
        return Results.Problem();
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Queries.GetImage;

namespace PostsApp.Controllers;

public static class ImageEndpoints 
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {
        var images = app.MapGroup("api/images");
        images.MapGet("{name}", Get);
    }
    
    public static async Task<IResult> Get(
        string name,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetImageQuery
        {
            ImageName = name
        };
        var result = await sender.Send(query, cancellationToken);
        return Results.File(
            result.FileStream,
            $"image/{result.FileInfo.Extension.Replace(".", "")}"
        );
    }
}
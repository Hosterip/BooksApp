using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Queries.GetImage;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public class ImageEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Images.Get, Get);
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
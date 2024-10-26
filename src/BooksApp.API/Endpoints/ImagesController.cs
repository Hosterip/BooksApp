using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Queries.GetImage;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public class ImagesController : ApiController
{
    private readonly ISender _sender;

    public ImagesController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet(ApiRoutes.Images.Get)]
    public async Task<IActionResult> Get(
        string name,
        CancellationToken cancellationToken)
    {
        var query = new GetImageQuery
        {
            ImageName = name
        };
        var result = await _sender.Send(query, cancellationToken);
        return File(
            result.FileStream,
            $"image/{result.FileInfo.Extension.Replace(".", "")}"
        );
    }
}
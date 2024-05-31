using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Queries.GetImage;
using PostsApp.Domain.Constants;

namespace PostsApp.Controllers;

[Route("Images")]
public class ImagesController : Controller
{
    private readonly ISender _sender;

    public ImagesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{imageName}")]
    public async Task<IActionResult> GetImage(string imageName, CancellationToken cancellationToken)
    {
        var query = new GetImageQuery
        {
            ImageName = imageName
        };
        var result = await _sender.Send(query, cancellationToken);
        return File(
            result.FileStream,
            $"image/{result.FileInfo.Extension.Replace(".", "")}"
        );
    }
}
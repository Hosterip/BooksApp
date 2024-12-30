using BooksApp.API.Common.Constants;
using BooksApp.Application.Images.Queries.GetImage;
using BooksApp.Contracts.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BooksApp.API.Controllers;

public class ImagesController(ISender sender) : ApiController
{
    [HttpGet(ApiRoutes.Images.Get)]
    [ProducesResponseType(typeof(FileStream), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var query = new GetImageQuery
        {
            ImageName = name
        };
        var result = await sender.Send(query, cancellationToken);
        return File(
            result.FileStream,
            $"image/{result.FileInfo.Extension.Trim('.')}"
        );
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Genres.Commands.CreateGenre;
using PostsApp.Application.Genres.Queries.GetAllGenres;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public class GenresController : ApiController
{
    private readonly ISender _sender;

    public GenresController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiRoutes.Genres.Create)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Create(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var createGenreCommand = new CreateGenreCommand
        {
            Name = name
        };
        var genre = await _sender.Send(createGenreCommand, cancellationToken);

        return Ok(genre);
    }

    [HttpGet(ApiRoutes.Genres.GetAll)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var getAllGenreQuery = new GetAllGenresQuery();
        var genres = await _sender.Send(getAllGenreQuery, cancellationToken);

        return Ok(genres);
    }
}
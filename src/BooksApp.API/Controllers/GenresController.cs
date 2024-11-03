using BooksApp.API.Common.Constants;
using BooksApp.Application.Genres;
using BooksApp.Application.Genres.Commands.CreateGenre;
using BooksApp.Application.Genres.Queries.GetAllGenres;
using BooksApp.Contracts.Responses.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksApp.API.Controllers;

public class GenresController : ApiController
{
    private readonly ISender _sender;

    public GenresController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiRoutes.Genres.Create)]
    [Authorize]
    [ProducesResponseType(typeof(GenreResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreResult>> Create(
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
    [ProducesResponseType(typeof(IEnumerable<GenreResult>),StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GenreResult>>> GetAll(
        CancellationToken cancellationToken)
    {
        var getAllGenreQuery = new GetAllGenresQuery();
        var genres = await _sender.Send(getAllGenreQuery, cancellationToken);

        return Ok(genres);
    }
}
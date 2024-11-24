using BooksApp.API.Common.Constants;
using BooksApp.Application.Genres;
using BooksApp.Application.Genres.Commands.CreateGenre;
using BooksApp.Application.Genres.Queries.GetAllGenres;
using BooksApp.Contracts.Errors;
using BooksApp.Contracts.Genres;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BooksApp.API.Controllers;

public class GenresController : ApiController
{
    private readonly ISender _sender;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IMapper _mapster;

    public GenresController(ISender sender, IOutputCacheStore outputCacheStore, IMapper mapster)
    {
        _sender = sender;
        _outputCacheStore = outputCacheStore;
        _mapster = mapster;
    }

    [HttpPost(ApiRoutes.Genres.Create)]
    [Authorize]
    [ProducesResponseType(typeof(GenreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreResponse>> Create(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var createGenreCommand = new CreateGenreCommand
        {
            Name = name
        };
        
        var genre = await _sender.Send(createGenreCommand, cancellationToken);

        await _outputCacheStore.EvictByTagAsync(OutputCache.Genres.Tag, cancellationToken);

        var response = _mapster.Map<GenreResponse>(genre);
        
        return Ok(response);
    }

    [HttpGet(ApiRoutes.Genres.GetAll)]
    [OutputCache(PolicyName = OutputCache.Genres.PolicyName)]
    [ProducesResponseType(typeof(IEnumerable<GenreResponse>),StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GenreResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var getAllGenreQuery = new GetAllGenresQuery();
        var genres = await _sender.Send(getAllGenreQuery, cancellationToken);
        
        var response = _mapster.Map<IEnumerable<GenreResponse>>(genres);
        
        return Ok(response);
    }
}
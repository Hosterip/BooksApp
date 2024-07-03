using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Genres.Commands.CreateGenre;
using PostsApp.Application.Genres.Queries.GetAllGenres;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

[Route("[controller]")]
public class GenresController : Controller
{
    private readonly ISender _sender;
    public GenresController (ISender sender)
    {
        _sender = sender;
    }
    [HttpPost("{name}")]
    [Authorize(Policy = Policies.Admin)]
    public async Task<IActionResult> Create(string name ,CancellationToken cancellationToken)
    {
        var createGenreCommand = new CreateGenreCommand
        {
            Name = name
        };
        var genre = await _sender.Send(createGenreCommand, cancellationToken);

        return StatusCode(201, genre);
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var getAllGenreQuery = new GetAllGenresQuery();
        var genres = await _sender.Send(getAllGenreQuery, cancellationToken);

        return StatusCode(201, genres);
    }
}
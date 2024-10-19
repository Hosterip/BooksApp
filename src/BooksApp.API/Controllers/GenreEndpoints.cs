using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Genres.Commands.CreateGenre;
using PostsApp.Application.Genres.Queries.GetAllGenres;
using PostsApp.Common.Constants;

namespace PostsApp.Controllers;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Genres.Create, Create)
            .RequireAuthorization(Policies.Admin);

        app.MapGet(ApiEndpoints.Genres.GetAll, GetAll);
    }
    
    public static async Task<IResult> Create(
        string name, 
        ISender sender,
        CancellationToken cancellationToken)
    {
        var createGenreCommand = new CreateGenreCommand
        {
            Name = name
        };
        var genre = await sender.Send(createGenreCommand, cancellationToken);

        return Results.Ok(genre);
    }
    
    public static async Task<IResult> GetAll(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var getAllGenreQuery = new GetAllGenresQuery();
        var genres = await sender.Send(getAllGenreQuery, cancellationToken);

        return Results.Ok(genres);
    }
}
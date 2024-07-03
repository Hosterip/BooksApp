using MediatR;

namespace PostsApp.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommand : IRequest<GenreResult>
{
    public required string Name { get; init; }
}
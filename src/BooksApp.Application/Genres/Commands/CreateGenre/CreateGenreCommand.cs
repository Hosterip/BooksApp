using MediatR;

namespace BooksApp.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommand : IRequest<GenreResult>
{
    public required string Name { get; init; }
}
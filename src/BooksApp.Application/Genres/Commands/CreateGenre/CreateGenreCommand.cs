using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Genres.Commands.CreateGenre;

[Authorize]
public class CreateGenreCommand : IRequest<GenreResult>
{
    public required string Name { get; init; }
}
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Genre;
using MediatR;

namespace BooksApp.Application.Genres.Commands.CreateGenre;

internal sealed class CreateGenreCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, GenreResult>
{
    public async Task<GenreResult> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = Genre.Create(request.Name);
        await unitOfWork.Genres.AddAsync(genre, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
        return new GenreResult
        {
            Id = genre.Id.Value,
            Name = genre.Name
        };
    }
}
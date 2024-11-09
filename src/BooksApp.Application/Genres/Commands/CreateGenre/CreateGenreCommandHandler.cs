using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Genre;
using MediatR;

namespace BooksApp.Application.Genres.Commands.CreateGenre;

internal sealed class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GenreResult> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = Genre.Create(request.Name);
        await _unitOfWork.Genres.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new GenreResult
        {
            Id = genre.Id.Value,
            Name = genre.Name
        };
    }
}
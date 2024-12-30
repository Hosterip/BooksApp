using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Genres.Queries.GetAllGenres;

internal class GetAllGenresQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllGenresQuery, IEnumerable<GenreResult>>
{
    public async Task<IEnumerable<GenreResult>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var rawGenres = await unitOfWork.Genres.GetAllAsync(cancellationToken);
        var genres = rawGenres
            .Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name });
        return genres;
    }
}
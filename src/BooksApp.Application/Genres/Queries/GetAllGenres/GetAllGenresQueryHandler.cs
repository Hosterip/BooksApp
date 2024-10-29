using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Genres.Queries.GetAllGenres;

public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, List<GenreResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGenresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<GenreResult>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var rawGenres = await _unitOfWork.Genres.GetAllAsync();
        var genres = rawGenres
            .Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
            .ToList();
        return genres;
    }
}
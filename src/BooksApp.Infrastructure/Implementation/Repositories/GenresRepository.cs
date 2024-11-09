using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Genre.ValueObjects;
using BooksApp.Infrastructure.Data;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class GenresRepository : GenericRepository<Genre>, IGenresRepository
{
    public GenresRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public IEnumerable<Genre?> GetAllByIds(IEnumerable<Guid> genreIds)
    {
        var parsedGenreIds = genreIds.Select(x => GenreId.CreateGenreId(x));
        var result = _dbContext.Genres.Where(x => parsedGenreIds.Contains(x.Id)).ToList();
        return result;
    }
}
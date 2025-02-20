using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Genre.ValueObjects;
using BooksApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Genres.Persistence;

public class GenresRepository : GenericRepository<Genre>, IGenresRepository
{
    public GenresRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Genre>> GetAllByIds(IEnumerable<Guid> genreIds, CancellationToken token = default)
    {
        var parsedGenreIds = genreIds.Select(x => GenreId.Create(x));
        var result = await DbContext.Genres
            .Where(x => parsedGenreIds.Contains(x.Id)).ToListAsync(token);
        return result;
    }
}
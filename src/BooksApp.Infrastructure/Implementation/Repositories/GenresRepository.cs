using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Genre;
using PostsApp.Domain.Genre.ValueObjects;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class GenresRepository : GenericRepository<Genre>, IGenresRepository
{
    public GenresRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public IEnumerable<Genre?> GetAllByIds(IEnumerable<Guid> genreIds)
    {
        var result = genreIds.Select(genreId =>
        {
            return _dbContext.Genres.FirstOrDefault(genre => genre.Id == GenreId.CreateGenreId(genreId));
        });
        return result;
    }
}
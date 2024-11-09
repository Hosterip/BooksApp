using BooksApp.Domain.Genre;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IGenresRepository : IGenericRepository<Genre>
{
    Task<IEnumerable<Genre>> GetAllByIds(IEnumerable<Guid> genreIds, CancellationToken token = default);
}
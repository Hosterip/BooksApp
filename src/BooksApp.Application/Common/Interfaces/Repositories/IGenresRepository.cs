using BooksApp.Domain;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IGenresRepository : IGenericRepository<Genre>
{
    Task<IEnumerable<Genre>> GetAllByIds(IEnumerable<Guid> genreIds, CancellationToken token = default);
}
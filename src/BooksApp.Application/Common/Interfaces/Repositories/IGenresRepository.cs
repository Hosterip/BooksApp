using BooksApp.Domain.Genre;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IGenresRepository : IGenericRepository<Genre>
{
    IEnumerable<Genre?> GetAllByIds(IEnumerable<Guid> genreIds);
}
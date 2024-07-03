using PostsApp.Domain.Genre;
using PostsApp.Domain.Genre.ValueObjects;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IGenresRepository : IGenericRepository<Genre>
{
    IEnumerable<Genre?> GetAllByIds(IEnumerable<Guid> genreIds);
}
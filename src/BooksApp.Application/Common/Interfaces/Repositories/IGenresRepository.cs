using PostsApp.Domain.Genre;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IGenresRepository : IGenericRepository<Genre>
{
    IEnumerable<Genre?> GetAllByIds(IEnumerable<Guid> genreIds);
}
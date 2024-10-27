using PostsApp.Domain.Bookshelf;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IBookshelvesRepository : IGenericRepository<Bookshelf>
{
    Task<bool> AnyById(Guid bookshelfId);
    Task<bool> AnyByName(string name, Guid userId);
    Task<bool> AnyBookById(Guid bookshelfId, Guid bookId);
    Task<bool> AnyBookByName(string name, Guid userId, Guid bookId);
    Task<Bookshelf?> GetSingleById(Guid bookshelfId);
}
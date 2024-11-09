using BooksApp.Domain.Bookshelf;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IBookshelvesRepository : IGenericRepository<Bookshelf>
{
    Task<bool> AnyById(Guid bookshelfId, CancellationToken token = default);
    Task<bool> AnyByName(string name, Guid userId, CancellationToken token = default);
    Task<bool> AnyBookById(Guid bookshelfId, Guid bookId, CancellationToken token = default);
    Task<bool> AnyBookByName(string name, Guid userId, Guid bookId, CancellationToken token = default);
    Task<Bookshelf?> GetBookshelfByName(string name, Guid userId, CancellationToken token = default);
    Task<Bookshelf?> GetSingleById(Guid bookshelfId, CancellationToken token = default);
}
using BooksApp.Domain.Bookshelf;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IBookshelvesRepository : IGenericRepository<Bookshelf>
{
    Task<bool> AnyById(Guid bookshelfId);
    Task<bool> AnyByName(string name, Guid userId);
    Task<bool> AnyBookById(Guid bookshelfId, Guid bookId);
    Task<bool> AnyBookByName(string name, Guid userId, Guid bookId);
    Task<Bookshelf?> GetBookshelfByName(string name, Guid userId);
    Task<Bookshelf?> GetSingleById(Guid bookshelfId);
}
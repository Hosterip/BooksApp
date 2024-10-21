using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Bookshelf;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IBookshelvesRepository : IGenericRepository<Bookshelf>
{
    Task<bool> AnyById(Guid bookshelfId);
    Task<bool> AnyByRefName(string refName, Guid userId);
    Task<bool> AnyBookById(Guid bookshelfId,Guid bookId);
    Task<bool> AnyBookByRefName(string refName, Guid userId, Guid bookId);
    Task<Bookshelf?> GetSingleById(Guid bookshelfId);
}
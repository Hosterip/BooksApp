using System.Linq.Expressions;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Book;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IBooksRepository : IGenericRepository<Book>
{
    Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page, Expression<Func<Book,bool>> expression);
    Task<PaginatedArray<BookResult>?>? GetPaginatedBookshelfBooks(Guid bookshelfId, int limit, int page);
    
    Task<Book?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
    Task<bool> AnyByRefName(Guid userId, string title);
    RatingStatistics RatingStatistics(Guid bookId);
}
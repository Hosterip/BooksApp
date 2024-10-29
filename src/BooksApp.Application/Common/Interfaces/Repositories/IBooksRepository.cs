using System.Linq.Expressions;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Results;
using BooksApp.Domain.Book;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IBooksRepository : IGenericRepository<Book>
{
    Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page, Expression<Func<Book, bool>> expression);
    Task<PaginatedArray<BookResult>?>? GetPaginatedBookshelfBooks(Guid bookshelfId, int limit, int page);

    Task<Book?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
    Task<bool> AnyByTitle(Guid userId, string title);
    RatingStatistics RatingStatistics(Guid bookId);
}
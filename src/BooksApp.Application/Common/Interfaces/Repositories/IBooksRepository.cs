using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Results;
using BooksApp.Domain.Book;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IBooksRepository : IGenericRepository<Book>
{
    Task<PaginatedArray<BookResult>> GetPaginated(
        Guid? currentUserId,
        int limit,
        int page,
        string? title,
        Guid? userId,
        Guid? genreId);

    Task<PaginatedArray<BookResult>?>? GetPaginatedBookshelfBooks(Guid? currentUserId, Guid bookshelfId, int limit,
        int page);

    Task<Book?> GetSingleById(Guid guid, CancellationToken token = default);
    Task<bool> AnyById(Guid guid, CancellationToken token = default);
    Task<bool> AnyByTitle(Guid userId, string title, CancellationToken token = default);
    RatingStatistics RatingStatistics(Guid bookId);
}
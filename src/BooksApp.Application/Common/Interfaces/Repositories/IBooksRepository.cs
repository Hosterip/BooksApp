using System.Linq.Expressions;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Book;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IBooksRepository : IGenericRepository<Book>
{
    Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page, Expression<Func<Book,bool>> expression);
    Task<IEnumerable<BookResult>> GetBooks(Expression<Func<Book, bool>> expression);
    double AverageRating(Guid bookId);
}
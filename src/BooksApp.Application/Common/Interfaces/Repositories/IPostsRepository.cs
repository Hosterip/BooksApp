using System.Linq.Expressions;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IPostsRepository : IGenericRepository<Book>
{
    Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page, string query);
    Task<IEnumerable<BookResult>> GetBooks(Expression<Func<Book, bool>> expression);
}
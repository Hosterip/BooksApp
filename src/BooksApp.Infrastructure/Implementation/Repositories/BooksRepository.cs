using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class BooksRepository : GenericRepository<Book>, IPostsRepository
{
    public BooksRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page, string query) 
    {
        return await 
            (
                from book in _dbContext.Books
                where query == null || book.Title.Contains(query)
                let user = new UserResult { Id = book.Author.Id, Username = book.Author.Username, Role = book.Author.Role.Name }
                join like in _dbContext.Likes.Include(like => like.Book)
                    on book.Id equals like.Book.Id into likes
                select new BookResult { Id = book.Id, Title = book.Title, Description = book.Description, Author = user, LikeCount = likes.Count()})
            .PaginationAsync(page, limit);
    }

    public async Task<IEnumerable<BookResult>> GetBooks(Expression<Func<Book, bool>> expression)
    {
        return (
            from book in _dbContext.Books
                .Where(expression)
                .Include(book => book.Author)
            
            join like in _dbContext.Likes.Include(like => like.Book)
                on book.Id equals like.Book.Id into likes 
            let user = new UserResult{ Id = book.Author.Id, Username = book.Author.Username, Role = book.Author.Role.Name }
            select new BookResult { Id = book.Id, Title = book.Title, Description = book.Description, LikeCount = likes.Count(), Author = user }
        );
    }

    public override async Task<Book?> GetSingleWhereAsync(Expression<Func<Book, bool>> expression)
    {
        var book = await _dbContext.Books
            .Include(book => book.Author)
            .SingleAsync(expression);

        return book;
    }

    public override async Task<bool> AnyAsync(Expression<Func<Book, bool>> expression)
    {
        return await _dbContext.Books
            .Include(book => book.Author)
            .AnyAsync(expression);
    }
}
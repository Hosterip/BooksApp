using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    public BooksRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page,
        Expression<Func<Book, bool>> expression)
    {
        var result = await
            (
                from book in _dbContext.Books
                    .Include(book => book.Author)
                    .Include(book => book.Author.Role)
                    .Where(expression)
                let user = new UserResult
                    { Id = book.Author.Id, Username = book.Author.Username, Role = book.Author.Role.Name }
                select new BookResult
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Author = user,
                    Average = -1
                }
            )
            .PaginationAsync(page, limit);
        result.Items = result.Items.Select(book =>
        {
            book.Average = AverageRating(book.Id);
            return book;
        }).ToArray();
        return result;
    }

    public async Task<IEnumerable<BookResult>> GetBooks(Expression<Func<Book, bool>> expression)
    {
        return (
            from book in _dbContext.Books
                .Include(book => book.Author)
                .Where(expression)
            let user = new UserResult
            {
                Id = book.Author.Id,
                Username = book.Author.Username,
                Role = book.Author.Role.Name
            }
            select new BookResult
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Author = user,
                Average = AverageRating(book.Id)
            }
        );
    }

    public double AverageRating(int bookId)
    {
        var reviews = _dbContext.Reviews
            .Include(review => review.Book)
            .Where(review => review.Book.Id == bookId);
        if (reviews.Any())
        {
            return reviews.Average(review => review.Rating);
        }

        return 0;
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
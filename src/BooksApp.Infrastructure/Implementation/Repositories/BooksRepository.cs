using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Book;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Bookshelf.ValueObjects;
using PostsApp.Domain.Common.Utils;
using PostsApp.Domain.Genre;
using PostsApp.Domain.User.ValueObjects;
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
                    .Where(expression)
                let user = new UserResult
                {
                    Id = book.Author.Id.Value.ToString(),
                    Username = book.Author.Username,
                    Role = book.Author.Role.Name,
                    AvatarName = book.Author.Avatar.ImageName
                }
                select new BookResult
                {
                    Id = book.Id.Value.ToString(),
                    Title = book.Title,
                    ReferentialName = book.ReferentialName,
                    Description = book.Description,
                    Author = user,
                    Average = -1,
                    CoverName = book.Cover.ImageName,
                    Genres = book.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
                        .ToList()
                }
            )
            .PaginationAsync(page, limit);
        result.Items = result.Items.Select(book =>
        {
            book.Average = AverageRating(new Guid(book.Id));
            return book;
        }).ToArray();
        return result;
    }

    public async Task<PaginatedArray<BookResult>?>? GetPaginatedBookshelfBooks(Guid bookshelfId, int limit, int page)
    {
        if (!await _dbContext.Bookshelves
                .AnyAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))) return null;
        var result = await _dbContext.Bookshelves
            .Include(bookshelf => bookshelf.BookshelfBooks)
            .Where(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .Select(bb =>  new BookResult
                {
                    Id = bb.Book.Id.Value.ToString(),
                    Title = bb.Book.Title,
                    ReferentialName = bb.Book.ReferentialName,
                    Description = bb.Book.Description,
                    Author = new UserResult
                    {
                        Id = bb.Book.Author.Id.Value.ToString(),
                        Username = bb.Book.Author.Username,
                        AvatarName = bb.Book.Author.Avatar == null ? null : bb.Book.Author.Avatar.ImageName,
                        Role = bb.Book.Author.Role.Name
                    },
                    Average = -1,
                    CoverName = bb.Book.Cover.ImageName,
                    Genres = bb.Book.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
                        .ToList()
                }
            ).PaginationAsync(page, limit);
        result.Items = result.Items.Select(book =>
        {
            book.Average = AverageRating(new Guid(book.Id));
            return book;
        }).ToArray();
        return result;

    }

    public async Task<Book?> GetSingleById(Guid guid)
    {
        return await _dbContext.Books.SingleOrDefaultAsync(book => book.Id == BookId.CreateBookId(guid));
    }

    public async Task<bool> AnyById(Guid guid)
    {
        return await _dbContext.Books.AnyAsync(book => book.Id == BookId.CreateBookId(guid));
    }

    public async Task<bool> AnyByRefName(Guid userId, string title)
    {
        return await _dbContext.Books.AnyAsync(book => book.Author.Id == UserId.CreateUserId(userId) &&
                                                       book.ReferentialName == title.ConvertToReferencial());
    }

    public double AverageRating(Guid bookId)
    {
        var reviews = _dbContext.Reviews
            .Include(review => review.Book)
            .Where(review => review.Book.Id == BookId.CreateBookId(bookId));
        if (reviews.Any())
        {
            return reviews.Average(review => review.Rating);
        }

        return 0;
    }
}
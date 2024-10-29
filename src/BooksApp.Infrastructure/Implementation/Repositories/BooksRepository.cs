using System.Linq.Expressions;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Extensions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Genres;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Book;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Utils;
using BooksApp.Domain.User.ValueObjects;
using BooksApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    public BooksRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page,
        Expression<Func<Book, bool>> expression)
    {
        var result = await
            _dbContext.Books.Where(expression).Select(book => new BookResult
                {
                    Id = book.Id.Value.ToString(),
                    Title = book.Title,
                    ReferentialName = book.ReferentialName,
                    Description = book.Description,
                    Author = new UserResult
                    {
                        Id = book.Author.Id.Value.ToString(),
                        Email = book.Author.Email,
                        FirstName = book.Author.FirstName,
                        MiddleName = book.Author.MiddleName,
                        LastName = book.Author.LastName,
                        Role = book.Author.Role.Name,
                        AvatarName = book.Author.Avatar.ImageName
                    },
                    AverageRating = -1,
                    Ratings = 0,
                    CoverName = book.Cover.ImageName,
                    Genres = book.Genres
                        .Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name }).ToList()
                })
                .PaginationAsync(page, limit);
        result.Items = result.Items.Select(book =>
        {
            var bookStats = RatingStatistics(new Guid(book.Id));
            book.Ratings = bookStats.Ratings;
            book.AverageRating = bookStats.AverageRating;
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
            .Select(bb => new BookResult
                {
                    Id = bb.Book.Id.Value.ToString(),
                    Title = bb.Book.Title,
                    ReferentialName = bb.Book.ReferentialName,
                    Description = bb.Book.Description,
                    Author = new UserResult
                    {
                        Id = bb.Book.Author.Id.Value.ToString(),
                        Email = bb.Book.Author.Email,
                        FirstName = bb.Book.Author.FirstName,
                        MiddleName = bb.Book.Author.MiddleName,
                        LastName = bb.Book.Author.LastName,
                        AvatarName = bb.Book.Author.Avatar == null ? null : bb.Book.Author.Avatar.ImageName,
                        Role = bb.Book.Author.Role.Name
                    },
                    AverageRating = -1,
                    Ratings = 0,
                    CoverName = bb.Book.Cover.ImageName,
                    Genres = bb.Book.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
                        .ToList()
                }
            ).PaginationAsync(page, limit);
        result.Items = result.Items.Select(book =>
        {
            var bookStats = RatingStatistics(new Guid(book.Id));
            book.Ratings = bookStats.Ratings;
            book.AverageRating = bookStats.AverageRating;
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

    public async Task<bool> AnyByTitle(Guid userId, string title)
    {
        var refTitle = title.GenerateRefName();
        return await _dbContext.Books.AnyAsync(book => book.Author.Id == UserId.CreateUserId(userId) &&
                                                       book.ReferentialName == refTitle);
    }

    public RatingStatistics RatingStatistics(Guid bookId)
    {
        var reviews = _dbContext.Reviews
            .Include(review => review.Book)
            .Where(review => review.Book.Id == BookId.CreateBookId(bookId));
        if (reviews.Any())
            return new RatingStatistics
            {
                AverageRating = reviews.Average(review => review.Rating),
                Ratings = reviews.Count()
            };

        return new RatingStatistics
        {
            AverageRating = 0,
            Ratings = 0
        };
    }
}
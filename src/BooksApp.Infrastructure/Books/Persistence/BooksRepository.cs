using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Extensions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Genres;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Book;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Helpers;
using BooksApp.Domain.Genre.ValueObjects;
using BooksApp.Domain.Review;
using BooksApp.Domain.User.ValueObjects;
using BooksApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Books.Persistence;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    public BooksRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<BookResult>> GetPaginated(
        Guid? currentUserId,
        int limit,
        int page,
        string? title,
        Guid? userId,
        Guid? genreId)
    {
        IQueryable<Book> bookQueryable = DbContext.Books
            .Include(b => b.Author.Followers)
            .Include(b => b.Author.Following);


        if (!string.IsNullOrWhiteSpace(title))
            bookQueryable = bookQueryable.Where(book => book.Title.Contains(title));

        if (userId != null)
            bookQueryable = bookQueryable.Where(book => book.Author.Id == UserId.CreateUserId(userId));

        if (genreId != null)
            bookQueryable =
                bookQueryable.Where(book => book.Genres.Any(genre => genre.Id == GenreId.CreateGenreId(genreId)));

        var result = await
            ConvertToBookResult(bookQueryable, DbContext.Reviews, currentUserId)
                .PaginationAsync(page, limit);
        return result;
    }

    public async Task<PaginatedArray<BookResult>?>? GetPaginatedBookshelfBooks(
        Guid? currentUserId,
        Guid bookshelfId,
        int limit,
        int page)
    {
        if (!await DbContext.Bookshelves
                .AnyAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))) return null;
        var queryable = DbContext.Bookshelves
            .Include(bookshelf => bookshelf.BookshelfBooks)
            .Where(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .Include(b => b.Book.Author.Followers)
            .Include(b => b.Book.Author.Following)
            .Select(bb => bb.Book);
        var result =
            await ConvertToBookResult(queryable, DbContext.Reviews, currentUserId)
                .PaginationAsync(page, limit);
        return result;
    }

    public async Task<Book?> GetSingleById(Guid guid, CancellationToken token = default)
    {
        return await DbContext.Books
            .SingleOrDefaultAsync(
                book => book.Id == BookId.CreateBookId(guid),
                cancellationToken: token);
    }

    public async Task<bool> AnyById(Guid guid, CancellationToken token = default)
    {
        return await DbContext.Books.AnyAsync(
            book => book.Id == BookId.CreateBookId(guid),
            cancellationToken: token);
    }

    public async Task<bool> AnyByTitle(Guid userId, string title, CancellationToken token = default)
    {
        var refTitle = title.GenerateRefName();
        return await DbContext.Books.AnyAsync(
            book => book.Author.Id == UserId.CreateUserId(userId) &&
                    book.ReferentialName == refTitle,
            cancellationToken: token);
    }

    public RatingStatistics RatingStatistics(Guid bookId)
    {
        var reviews = DbContext.Reviews
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

    private static RatingStatistics RatingsStatistics(IEnumerable<int> reviews)
    {
        var array = reviews as int[] ?? reviews.ToArray();
        if (array.Length != 0)
            return new RatingStatistics
            {
                AverageRating = array.Average(),
                Ratings = array.Length
            };

        return new RatingStatistics
        {
            AverageRating = 0,
            Ratings = 0
        };
    }

    private static IQueryable<BookResult> ConvertToBookResult(IQueryable<Book> books,
        IQueryable<Review> reviewsQueryable, Guid? currentUserId)
    {
        return (
            from book in books
            join review in reviewsQueryable
                    .Include(x => x.Book)
                    .Select(x => new { x.Book, x.Rating })
                on book.Id equals review.Book.Id into reviews
            let viewerRelationship = book.Author.ViewerRelationship(currentUserId)
            let ratingStatistics = RatingsStatistics(reviews.Select(x => x.Rating))
            select new BookResult
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
                    AvatarName = book.Author.Avatar == null ? null : book.Author.Avatar.ImageName,
                    Role = book.Author.Role.Name,
                    ViewerRelationship = currentUserId == null
                        ? null
                        : new ViewerRelationship
                        {
                            IsFollowing = viewerRelationship.IsFollowing,
                            IsFriend = viewerRelationship.IsFriend,
                            IsMe = viewerRelationship.IsMe
                        }
                },
                AverageRating = ratingStatistics.AverageRating,
                Ratings = ratingStatistics.Ratings,
                CoverName = book.Cover.ImageName,
                Genres = book.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
            });
    }
}
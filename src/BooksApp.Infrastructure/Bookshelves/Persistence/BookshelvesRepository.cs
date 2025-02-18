using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Helpers;
using BooksApp.Domain.User.ValueObjects;
using BooksApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Bookshelves.Persistence;

public class BookshelvesRepository : GenericRepository<Bookshelf>, IBookshelvesRepository
{
    public BookshelvesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> AnyById(Guid bookshelfId, CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .AnyAsync(
                bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId), 
                cancellationToken: token);
    }

    public async Task<bool> AnyByName(string name, Guid userId, CancellationToken token = default)
    {
        var refName = name.GenerateRefName();
        return await DbContext.Bookshelves
            .AnyAsync(
                bookshelf => bookshelf.UserId == UserId.CreateUserId(userId) &&
                             bookshelf.ReferentialName == refName,
                cancellationToken: token);
    }

    public async Task<bool> AnyBookById(Guid bookshelfId, Guid bookId, CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .Where(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(
                book => book.Book.Id == BookId.CreateBookId(bookId),
                cancellationToken: token);
    }

    public async Task<bool> AnyBookByName(string name, Guid userId, Guid bookId, CancellationToken token = default)
    {
        var refName = name.GenerateRefName();
        return await DbContext.Bookshelves
            .Where(bookshelf => bookshelf.ReferentialName == refName &&
                                bookshelf.UserId == UserId.CreateUserId(userId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId),
                cancellationToken: token);
    }

    public async Task<Bookshelf?> GetBookshelfByName(string name, Guid userId, CancellationToken token = default)
    {
        var refName = name.GenerateRefName();
        return await DbContext.Bookshelves
            .FirstOrDefaultAsync(
                bookshelf => bookshelf.UserId == UserId.CreateUserId(userId) &&
                             bookshelf.ReferentialName == refName,
                cancellationToken: token);
    }

    public async Task<Bookshelf?> GetSingleById(Guid bookshelfId, CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .AsSplitQuery()
            .SingleOrDefaultAsync(
                bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId),
                cancellationToken: token);
    }
}
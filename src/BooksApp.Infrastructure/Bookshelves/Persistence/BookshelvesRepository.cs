using System.Linq.Expressions;
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

    public override async Task<bool> AnyAsync(
        Expression<Func<Bookshelf, bool>> expression,
        CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .Include(x => x.User)
            .AnyAsync(expression, token);
    }

    public override async Task<IEnumerable<Bookshelf>> GetAllWhereAsync(
        Expression<Func<Bookshelf, bool>> expression,
        CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .Include(x => x.User)
            .Where(expression)
            .ToListAsync(token);
    }

    public override async Task<Bookshelf?> GetSingleWhereAsync(
        Expression<Func<Bookshelf, bool>> expression,
        CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .Include(x => x.User)
            .SingleOrDefaultAsync(expression, token);
    }

    public async Task<bool> AnyById(Guid bookshelfId, CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .AnyAsync(
                bookshelf => bookshelf.Id == BookshelfId.Create(bookshelfId),
                token);
    }

    public async Task<bool> AnyByName(string name, Guid userId, CancellationToken token = default)
    {
        var refName = name.GenerateRefName();
        return await DbContext.Bookshelves
            .Include(x => x.User)
            .AnyAsync(
                bookshelf => bookshelf.User.Id == UserId.Create(userId) &&
                             bookshelf.ReferentialName == refName,
                token);
    }

    public async Task<bool> AnyBookById(Guid bookshelfId, Guid bookId, CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .Where(bookshelf => bookshelf.Id == BookshelfId.Create(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(
                book => book.Book.Id == BookId.Create(bookId),
                token);
    }

    public async Task<bool> AnyBookByName(string name, Guid userId, Guid bookId, CancellationToken token = default)
    {
        var refName = name.GenerateRefName();
        return await DbContext.Bookshelves
            .Include(x => x.User)
            .Where(bookshelf => bookshelf.ReferentialName == refName &&
                                bookshelf.User.Id == UserId.Create(userId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.Create(bookId),
                token);
    }

    public async Task<Bookshelf?> GetBookshelfByName(string name, Guid userId, CancellationToken token = default)
    {
        var refName = name.GenerateRefName();
        return await DbContext.Bookshelves
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                bookshelf => bookshelf.User.Id == UserId.Create(userId) &&
                             bookshelf.ReferentialName == refName,
                token);
    }

    public async Task<Bookshelf?> GetSingleById(Guid bookshelfId, CancellationToken token = default)
    {
        return await DbContext.Bookshelves
            .AsSplitQuery()
            .SingleOrDefaultAsync(
                bookshelf => bookshelf.Id == BookshelfId.Create(bookshelfId),
                token);
    }
}
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Utils;
using BooksApp.Domain.User.ValueObjects;
using BooksApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class BookshelvesRepository : GenericRepository<Bookshelf>, IBookshelvesRepository
{
    public BookshelvesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> AnyById(Guid bookshelfId)
    {
        return await _dbContext.Bookshelves
            .AnyAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId));
    }

    public async Task<bool> AnyByName(string name, Guid userId)
    {
        var refName = name.GenerateRefName();
        return await _dbContext.Bookshelves
            .AnyAsync(bookshelf => bookshelf.User != null &&
                                   bookshelf.User.Id == UserId.CreateUserId(userId) &&
                                   bookshelf.ReferentialName == refName);
    }

    public async Task<bool> AnyBookById(Guid bookshelfId, Guid bookId)
    {
        return await _dbContext.Bookshelves
            .Where(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId));
    }

    public async Task<bool> AnyBookByName(string name, Guid userId, Guid bookId)
    {
        var refName = name.GenerateRefName();
        return await _dbContext.Bookshelves
            .Where(bookshelf => bookshelf.User != null &&
                                bookshelf.ReferentialName == refName &&
                                bookshelf.User.Id == UserId.CreateUserId(userId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId));
    }

    public async Task<Bookshelf?> GetBookshelfByName(string name, Guid userId)
    {
        var refName = name.GenerateRefName();
        return await _dbContext.Bookshelves
            .SingleOrDefaultAsync(bookshelf => bookshelf.User != null &&
                                   bookshelf.User.Id == UserId.CreateUserId(userId) &&
                                   bookshelf.ReferentialName == refName);
        
    }

    public async Task<Bookshelf?> GetSingleById(Guid bookshelfId)
    {
        return await _dbContext.Bookshelves
            .SingleOrDefaultAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId));
    }
}
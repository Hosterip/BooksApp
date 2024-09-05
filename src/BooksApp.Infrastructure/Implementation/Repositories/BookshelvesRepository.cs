using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.Bookshelf.ValueObjects;
using PostsApp.Domain.Common.Utils;
using PostsApp.Domain.User.ValueObjects;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

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

    public async Task<bool> AnyByRefName(Guid userId, string name)
    {
        return await _dbContext.Bookshelves
            .AnyAsync(bookshelf => bookshelf.User != null &&
                                   bookshelf.User.Id == UserId.CreateUserId(userId) &&
                                   bookshelf.ReferentialName == name.ConvertToReferencial());
    }

    public async Task<bool> AnyBookById(Guid bookshelfId, Guid bookId)
    {
        return await _dbContext.Bookshelves
            .Where(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId));
    }

    public async Task<bool> AnyBookById(string bookshelfName, Guid userId, Guid bookId)
    {
        return await _dbContext.Bookshelves
            .Where(bookshelf => bookshelf.User != null &&
                                bookshelf.Name == bookshelfName.Trim().ToLowerInvariant() && 
                                bookshelf.User.Id == UserId.CreateUserId(userId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId));
    }

    public async Task<Bookshelf?> GetSingleById(Guid bookshelfId)
    {
        return await _dbContext.Bookshelves
            .SingleOrDefaultAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId));
    }

    
}
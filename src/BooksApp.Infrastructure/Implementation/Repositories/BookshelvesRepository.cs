using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.Bookshelf.ValueObjects;
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
        return await _dbContext.Bookshelfes
            .AnyAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId));
    }

    public async Task<bool> AnyBookById(Guid bookshelfId, Guid bookId)
    {
        return await _dbContext.Bookshelfes
            .Where(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId));
    }

    public async Task<bool> AnyBookById(string bookshelfName, Guid userId, Guid bookId)
    {
        return await _dbContext.Bookshelfes
            .Where(bookshelf => bookshelf.User != null &&
                                bookshelf.Name == bookshelfName && 
                                bookshelf.User.Id == UserId.CreateUserId(userId))
            .SelectMany(bookshelf => bookshelf.BookshelfBooks)
            .AnyAsync(book => book.Book.Id == BookId.CreateBookId(bookId));
    }

    public async Task<Bookshelf?> GetSingleById(Guid bookshelfId)
    {
        return await _dbContext.Bookshelfes
            .SingleOrDefaultAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId));
    }

    
}
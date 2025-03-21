using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Infrastructure.Books.Persistence;
using BooksApp.Infrastructure.Bookshelves.Persistence;
using BooksApp.Infrastructure.Genres.Persistence;
using BooksApp.Infrastructure.Images.Persistence;
using BooksApp.Infrastructure.Reviews.Persistence;
using BooksApp.Infrastructure.Roles.Persistence;
using BooksApp.Infrastructure.Users.Persistence;

namespace BooksApp.Infrastructure.Common.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        Books = new BooksRepository(dbContext);
        Users = new UsersRepository(dbContext);
        Roles = new RolesRepository(dbContext);
        Reviews = new ReviewsRepository(dbContext);
        Images = new ImagesRepository(dbContext);
        Genres = new GenresRepository(dbContext);
        Bookshelves = new BookshelvesRepository(dbContext);
        _dbContext = dbContext;
    }

    public IBooksRepository Books { get; }
    public IUsersRepository Users { get; }
    public IRolesRepository Roles { get; }
    public IReviewsRepository Reviews { get; }
    public IImagesRepository Images { get; }
    public IGenresRepository Genres { get; }
    public IBookshelvesRepository Bookshelves { get; }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
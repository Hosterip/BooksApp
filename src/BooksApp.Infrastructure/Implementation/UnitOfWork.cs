using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Infrastructure.Data;
using PostsApp.Infrastructure.Implementation.Repositories;

namespace PostsApp.Infrastructure.Implementation;

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
        _dbContext = dbContext;
    }
    public IBooksRepository Books { get; private set; }
    public IUsersRepository Users { get; private set; }
    public IRolesRepository Roles { get; private set; }
    public IReviewsRepository Reviews { get; private set; }
    public IImagesRepository Images { get; private set; }
    public IGenresRepository Genres { get; }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Infrastructure.Data;
using PostsApp.Infrastructure.Implementation.Repositories;

namespace PostsApp.Infrastructure.Implementation;

public class UnitOfWork : IUnitOfWork
{
    public AppDbContext _dbContext { get; private set; }
    public UnitOfWork(AppDbContext dbContext)
    {
        Posts = new BooksRepository(dbContext);
        Users = new UsersRepository(dbContext);
        Likes = new LikesRepository(dbContext);
        Roles = new RolesRepository(dbContext);
        _dbContext = dbContext;
    }
    public IPostsRepository Posts { get; private set; }
    public IUsersRepository Users { get; private set; }
    public ILikesRepository Likes { get; private set; }
    public IRolesRepository Roles { get; private set; }
    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
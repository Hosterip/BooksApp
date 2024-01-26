using PostsApp.Application.Common.Interfaces;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure.Implementation;

public class UnitOfWork : IUnitOfWork
{
    public AppDbContext _dbContext { get; private set; }
    public UnitOfWork(AppDbContext dbContext)
    {
        Post = new PostRepository(dbContext);
        User = new UserRepository(dbContext);
        _dbContext = dbContext;
    }
    public IPostRepository Post { get; private set; }
    public IUserRepository User { get; private set; }
    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
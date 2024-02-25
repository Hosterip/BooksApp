using PostsApp.Application.Common.Interfaces.Repositories;

namespace PostsApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IBooksRepository Books { get; }
    public IUsersRepository Users { get; }
    public ILikesRepository Likes { get; }
    public IRolesRepository Roles { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}
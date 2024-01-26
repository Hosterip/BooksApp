namespace PostsApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IPostRepository Post { get; }
    public IUserRepository User { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}
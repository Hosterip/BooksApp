namespace PostsApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IPostsRepository Posts { get; }
    public IUsersRepository Users { get; }
    public ILikesRepository Likes { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}
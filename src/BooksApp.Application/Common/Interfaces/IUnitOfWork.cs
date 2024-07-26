using PostsApp.Application.Common.Interfaces.Repositories;

namespace PostsApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IBooksRepository Books { get; }
    public IUsersRepository Users { get; }
    public IRolesRepository Roles { get; }
    public IReviewsRepository Reviews { get; }
    public IImagesRepository Images { get; }
    public IGenresRepository Genres { get; }
    public IBookshelvesRepository Bookshelves { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}
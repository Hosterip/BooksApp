using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.User;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<PaginatedArray<UserResult>> GetPaginated(Guid? currentUserId, int page, int limit, string query);

    Task<PaginatedArray<UserResult>> GetPaginatedFollowers(Guid? currentUserId, Guid userId, int page, int limit,
        string query);

    Task<PaginatedArray<UserResult>> GetPaginatedFollowing(Guid? currentUserId, Guid userId, int page, int limit,
        string query);

    Task<User?> GetSingleById(Guid guid, CancellationToken token = default, bool includeRelationships = false,
        bool asTracking = false);

    Task<bool> AnyById(Guid guid, CancellationToken token = default);
    Task<bool> AnyByEmail(string email, CancellationToken token = default);
    Task<int> CountFollowers(Guid userId, CancellationToken token = default);
}
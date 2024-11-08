using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.User;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<PaginatedArray<UserResult>> GetPaginated(Guid? currentUserId, int page, int limit, string query);
    Task<PaginatedArray<UserResult>> GetPaginatedFollowers(Guid? currentUserId, Guid userId, int page, int limit, string query);
    Task<PaginatedArray<UserResult>> GetPaginatedFollowing(Guid? currentUserId, Guid userId, int page, int limit, string query);
    Task<User?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
    Task<bool> AnyByEmail(string email);
    Task<bool> AnyFollower(Guid userId, Guid followerId);
    Task AddFollower(Guid userId, Guid followerId);
    Task RemoveFollower(Guid userId, Guid followerId);
    Task<int> CountFollowers(Guid userId);
}
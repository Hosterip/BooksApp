using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.User;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query, Guid currentUserId = default);
    Task<User?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
    Task<bool> AnyByEmail(string email);
    Task<bool> AnyFollower(Guid userId, Guid followerId);
    Task AddFollower(Guid userId, Guid followerId);
}
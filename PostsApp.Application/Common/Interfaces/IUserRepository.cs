using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query);
    Task<bool> IsUsernameUnique(string username);
}
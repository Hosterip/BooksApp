using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.User;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query);
    Task<User?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
}
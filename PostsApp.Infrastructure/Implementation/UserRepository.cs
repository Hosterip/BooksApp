using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure.Implementation;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query)
    {
        return await 
            (
                from user in _dbContext.Users
                where query == null || user.Username.Contains(query)
                select new UserResult{Username = user.Username})
            .PaginationAsync(page, limit);
    }

    public async Task<bool> IsUsernameUnique(string username)
    {
        return !await _dbContext.Users.AnyAsync(user => user.Username == username);
    }
}
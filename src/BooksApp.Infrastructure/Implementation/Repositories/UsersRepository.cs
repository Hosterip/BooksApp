using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.User;
using PostsApp.Domain.User.ValueObjects;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class UsersRepository : GenericRepository<User>, IUsersRepository
{
    public UsersRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query)
    {
        return await 
            (
                from user in _dbContext.Users
                where query == null || user.FirstName.Contains(query)
                select new UserResult
                {
                    Id = user.Id.Value.ToString(),
                    Username = user.FirstName,
                    Role = user.Role.Name,
                    AvatarName = user.Avatar.ImageName ?? null
                })
            .PaginationAsync(page, limit);
    }

    public async Task<User?> GetSingleById(Guid guid)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == UserId.CreateUserId(guid));
    }

    public async Task<bool> AnyById(Guid guid)
    {
        return await _dbContext.Users.AnyAsync(user => user.Id == UserId.CreateUserId(guid));
    }
}
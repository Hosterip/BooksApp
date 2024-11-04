using System.ComponentModel.DataAnnotations;
using BooksApp.Application.Common.Extensions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.User;
using BooksApp.Domain.User.ValueObjects;
using BooksApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class UsersRepository : GenericRepository<User>, IUsersRepository
{
    public UsersRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query)
    {
        return await
            (
                from user in _dbContext.Users
                where query == null || user.FirstName.Contains(query)
                select new UserResult
                {
                    Id = user.Id.Value.ToString(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
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

    public async Task<bool> AnyByEmail(string email)
    {   
        if (new EmailAddressAttribute().IsValid(email) != true) return false;
        var parsedEmail = email.Trim().ToLower();
        return await _dbContext.Users.AnyAsync(user => user.Email == parsedEmail);
    }

    public async Task<bool> AnyFollower(Guid userId, Guid followerId)
    {
        return
            await _dbContext.Users
                .Take(1)
                .Where(u => u.Id == UserId.CreateUserId(userId))
                .SelectMany(u => u.Followers)
                .AnyAsync(f => f.FollowerId == UserId.CreateUserId(followerId));

    }

    public async Task AddFollower(Guid userId, Guid followerId)
    {
        var user = await GetSingleById(userId);
        var follower = await GetSingleById(followerId);
        if (user != null && follower != null)
            user.AddFollower(follower);
    }
}
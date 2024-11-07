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

    public async Task<PaginatedArray<UserResult>> GetPaginated(
        Guid? currentUserId, 
        int page,
        int limit,
        string query)
    {
        return await (
            from user in _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Followers)
            where query == null || user.FirstName.Contains(query)
            let viewerRelationship = user.ViewerRelationship(currentUserId)
            select new UserResult
                {
                    Id = user.Id.Value.ToString(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Role = user.Role.Name,
                    AvatarName = user.Avatar.ImageName ?? null,
                    ViewerRelationship = new ViewerRelationship
                    {
                        IsFollowing = viewerRelationship.IsFollowing,
                        IsFriend = viewerRelationship.IsFriend,
                        IsMe = viewerRelationship.IsMe
                    }
                })
            .PaginationAsync(page, limit);
    }

    public async Task<PaginatedArray<UserResult>> GetPaginatedFollowers(
        Guid? currentUserId,
        Guid userId,
        int page,
        int limit,
        string query)
    {
        
        return await (
                from relationship in _dbContext.Relationships
                    .AsNoTracking()
                    .Where(x => x.UserId == UserId.CreateUserId(userId))
                    .Include(x => x.UserId)
                    .Include(x => x.FollowerId)
                join user in _dbContext.Users
                        .Include(x => x.Followers)
                        .Include(x => x.Following)
                    on relationship.FollowerId equals user.Id  
                where query == null || user.FirstName.Contains(query)
                let viewerRelationship = user.ViewerRelationship(currentUserId)
                select new UserResult
                {
                    Id = user.Id.Value.ToString(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Role = user.Role.Name,
                    AvatarName = user.Avatar.ImageName ?? null,
                    ViewerRelationship = new ViewerRelationship
                    {
                        IsFollowing = viewerRelationship.IsFollowing,
                        IsFriend = viewerRelationship.IsFriend,
                        IsMe = viewerRelationship.IsMe
                    }
                })
            .PaginationAsync(page, limit);
    }

    public async Task<User?> GetSingleById(Guid guid)
    {
        return await _dbContext.Users
            .Include(x => x.Followers)
            .Include(x => x.Following)
            .SingleOrDefaultAsync(user => user.Id == UserId.CreateUserId(guid));
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

    public async Task RemoveFollower(Guid userId, Guid followerId)
    {
        var user = await GetSingleById(userId);
        var follower = await GetSingleById(followerId);
        if (user != null && follower != null)
            user.RemoveFollower(follower);
    }

    public async Task<int> CountFollowers(Guid userId)
    {
        var parsedUserId = UserId.CreateUserId(userId);
        return await _dbContext.Users
            .Include(x => x.Followers)
            .Take(1)
            .Where(x => x.Id == parsedUserId)
            .SelectMany(x => x.Followers)
            .CountAsync();
    }
}
using System.ComponentModel.DataAnnotations;
using BooksApp.Application.Common.Extensions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.User;
using BooksApp.Domain.User.ValueObjects;
using BooksApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Users.Persistence;

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
        IQueryable<User> queryable = DbContext.Users
            .AsNoTracking()
            .Include(x => x.Followers);

        if (!string.IsNullOrWhiteSpace(query))
            queryable = queryable.Where(x => x.FirstName.Contains(query));

        var result = await ConvertToUserResult(queryable, currentUserId)
            .PaginationAsync(page, limit);

        return result;
    }

    public async Task<PaginatedArray<UserResult>> GetPaginatedFollowers(
        Guid? currentUserId,
        Guid userId,
        int page,
        int limit,
        string query)
    {
        return await (
                from relationship in DbContext.Users
                    .Where(x => x.Id == UserId.Create(userId))
                    .Include(x => x.Followers)
                    .SelectMany(x => x.Followers)
                join user in DbContext.Users 
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
                    ViewerRelationship = currentUserId == null
                        ? null
                        : new ViewerRelationship
                        {
                            IsFollowing = viewerRelationship.IsFollowing,
                            IsFriend = viewerRelationship.IsFriend,
                            IsMe = viewerRelationship.IsMe
                        }
                })
            .PaginationAsync(page, limit);
    }

    public async Task<PaginatedArray<UserResult>> GetPaginatedFollowing(
        Guid? currentUserId,
        Guid userId,
        int page,
        int limit,
        string query)
    {
        return await (
                from relationship in DbContext.Users
                    .Where(x => x.Id == UserId.Create(userId))
                    .Include(x => x.Following)
                    .SelectMany(x => x.Following)
                join user in DbContext.Users 
                    on relationship.UserId equals user.Id
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
                    ViewerRelationship = currentUserId == null
                        ? null
                        : new ViewerRelationship
                        {
                            IsFollowing = viewerRelationship.IsFollowing,
                            IsFriend = viewerRelationship.IsFriend,
                            IsMe = viewerRelationship.IsMe
                        }
                })
            .PaginationAsync(page, limit);
    }

    public async Task<User?> GetSingleById(
        Guid guid,
        CancellationToken token = default,
        bool includeRelationships = false,
        bool asTracking = false)
    {
        var query = DbContext.Users.AsNoTracking();
        if (asTracking)
            query = query.AsTracking();
        if (includeRelationships)
            query = query
                .Include(x => x.Followers)
                .Include(x => x.Following);
        return await query.FirstOrDefaultAsync(x => x.Id == UserId.Create(guid), token);

    }

    public async Task<bool> AnyById(
        Guid guid,
        CancellationToken token = default)
    {
        return await DbContext.Users
            .AnyAsync(
            user => user.Id == UserId.Create(guid),
            cancellationToken: token);
    }

    public async Task<bool> AnyByEmail(
        string email,
        CancellationToken token = default)
    {
        if (new EmailAddressAttribute().IsValid(email) != true) return false;
        var parsedEmail = email.Trim().ToLower();
        return await DbContext.Users.AnyAsync(user => user.Email == parsedEmail, cancellationToken: token);
    }

    public async Task<bool> AnyFollower(
        Guid userId,
        Guid followerId,
        CancellationToken token = default)
    {
        return
            await DbContext.Users
                .Take(1)
                .Where(u => u.Id == UserId.Create(userId))
                .SelectMany(u => u.Followers)
                .AnyAsync(
                    f => f.FollowerId == UserId.Create(followerId),
                    cancellationToken: token);
    }

    public async Task<int> CountFollowers(
        Guid userId,
        CancellationToken token = default)
    {
        var parsedUserId = UserId.Create(userId);
        return await DbContext.Users
            .Include(x => x.Followers)
            .Take(1)
            .Where(x => x.Id == parsedUserId)
            .SelectMany(x => x.Followers)
            .CountAsync(cancellationToken: token);
    }

    private static IQueryable<UserResult> ConvertToUserResult(IQueryable<User> users, Guid? currentUserId)
    {
        return (
            from user in users
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
                ViewerRelationship = currentUserId == null
                    ? null
                    : new ViewerRelationship
                    {
                        IsFollowing = viewerRelationship.IsFollowing,
                        IsFriend = viewerRelationship.IsFriend,
                        IsMe = viewerRelationship.IsMe
                    }
            });
    }
}
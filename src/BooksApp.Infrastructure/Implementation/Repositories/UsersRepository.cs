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
        return await _dbContext.Users.AnyAsync(user => user.Email == email.ToLower());
    }
}
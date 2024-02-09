using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class LikesRepository : GenericRepository<Like>, ILikesRepository
{
    public LikesRepository(AppDbContext dbContext) : base(dbContext) { }
    public override async Task<bool> AnyAsync(Expression<Func<Like, bool>> expression)
    {
        return await _dbContext.Likes
            .Include(like => like.User)
            .Include(like => like.User)
            .Include(like => like.Book)
            .AnyAsync(expression);
    }
    public override async Task<Like?> GetSingleWhereAsync(Expression<Func<Like, bool>> expression)
    {
        return await _dbContext.Likes
            .Include(like => like.User)
            .Include(like => like.Book)
            .SingleOrDefaultAsync(expression);
    }
    public override async Task<IEnumerable<Like>> GetAllWhereAsync(Expression<Func<Like, bool>> expression)
    {
        return _dbContext.Likes
            .Include(like => like.User)
            .Include(like => like.Book)
            .Where(expression);
    }
    
}